/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: NavMenu.razor.cs
    **
    ** Description:
    **      Top level menu
    **
    ** This program is free software: you can redistribute it and/or modify
    ** it under the terms of the GNU General Public License version 3 as
    ** published by the Free Software Foundation.   
    **
    ** This program is distributed in the hope that it will be useful,
    ** but WITHOUT ANY WARRANTY; without even the implied warranty of
    ** MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
    ** GNU General Public License version 3 for more details.
    **
    **  You should have received a copy of the GNU General Public License
    **  version 3 along with this program in file "license-gpl-3.0.txt".
    **  If not, see <http://www.gnu.org/licenses/gpl-3.0.txt>.
    **
    **--------------------------------------------------------------------------*/

using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Notes2022.RCL.User.Dialogs;
using Notes2022.Shared;
using Syncfusion.Blazor.Navigations;
using System.Net.Http.Json;
using System.Timers;

namespace Notes2022.RCL.User.Menus
{
    /// <summary>
    /// Displays the top of page navigation bar
    /// </summary>
    public partial class NavMenu
    {
        /// <summary>
        /// For display of error message during initialization
        /// </summary>
        [CascadingParameter] public IModalService Modal { get; set; }

        [Parameter] public bool IsPreview { get; set; } = false;

        /// <summary>
        /// The list of menu bar items (structure of the menu)
        /// </summary>
        protected static List<MenuItem> menuItemsTop { get; set; }

        /// <summary>
        /// Root menu item
        /// </summary>
        protected SfMenu<MenuItem> topMenu { get; set; }

        /// <summary>
        /// Current time
        /// </summary>
        private string mytime { get; set; }

        /// <summary>
        /// Used to compare time and abort re-render in same minute
        /// </summary>
        private string mytime2 { get; set; } = "";
        
        /// <summary>
        /// Used to update menu bar time - tick once per second
        /// </summary>
        private System.Timers.Timer timer2 { get; set; }

        private bool collapseNavMenu = true;

        private string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        [Inject] HttpClient Http { get; set; }
        [Inject] AuthenticationStateProvider AuthProv { get; set; }
        [Inject] NavigationManager Navigation { get; set; }
        [Inject] Blazored.SessionStorage.ISessionStorageService sessionStorage { get; set; }
        public NavMenu()
        {
        }

        private void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }

        /// <summary>
        /// Update the clock once per second
        /// </summary>
        /// <param name="firstRender"></param>
        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                timer2 = new System.Timers.Timer(1000);
                timer2.Elapsed += TimerTick2;
                timer2.Enabled = true;
            }
        }

        /// <summary>
        /// Invoked once per second
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void TimerTick2(Object source, ElapsedEventArgs e)
        {
            mytime = DateTime.Now.ToShortTimeString();
            if (mytime != mytime2) // do we need to re-render?
            {
                StateHasChanged();
                mytime2 = mytime;
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            await UpdateMenu();
        }

        /// <summary>
        /// Enable only items available to logged in user
        /// </summary>
        /// <returns></returns>
        public async Task UpdateMenu()
        {

            bool isAdmin = false;
            bool isUser = false;

            if (IsPreview)
                goto IsPreview;

            AuthenticationState authstate = await AuthProv.GetAuthenticationStateAsync();
            if (authstate.User.Identity.IsAuthenticated)
            {
                try
                {
                    if (Globals.EditUserVModel is null) // do we alreay have user Info?
                    {
                        // NO - get it.
                        UserData udata = await DAL.GetUserData(Http);
                        string uid = udata.UserId;
                        Globals.UserData = udata;
                        Globals.EditUserVModel = await DAL.GetUserEdit(Http, uid);
                    }

                    if (Globals.RolesValid) // have we set roles already?
                        goto Found;         // yes

                    // Set user roles
                    foreach (CheckedUser u in Globals.EditUserVModel.RolesList)
                    {
                        if (u.theRole.NormalizedName == "ADMIN" && u.isMember)
                        {
                            isUser = isAdmin = true;
                        }
                        if (u.theRole.NormalizedName == "USER" && u.isMember)
                        {
                            isUser = true;
                        }
                    }

                    Globals.IsAdmin = isAdmin;
                    Globals.IsUser = isUser;
                    Globals.RolesValid = true;

                Found:
                    isAdmin = Globals.IsAdmin;
                    isUser = Globals.IsUser;
                }
                catch (Exception e)
                {
                    ShowMessage("In NavMenu: " + e.Message);
                }
            }

            IsPreview:

            // make the whole menu
            menuItemsTop = new List<MenuItem>();
            MenuItem item;

            item = new() { Id = "Recent", Text = "Recent Notes" };
            menuItemsTop.Add(item);

            MenuItem item3 = new() { Id = "Manage", Text = "Manage" };
            item3.Items = new List<MenuItem>
            {
                new () { Id = "MRecent", Text = "Recent" },
                new () { Id = "Subscriptions", Text = "Subscriptions" },
                new () { Id = "Preferences", Text = "Preferences" }
            };
            menuItemsTop.Add(item3);

            item = new() { Id = "Help", Text = "Help" };
            item.Items = new List<MenuItem>
            {
                new () { Id = "MainHelp", Text = "Help" },
                new () { Id = "About", Text = "About" },
                new () { Id = "License", Text = "License" }
            };
            menuItemsTop.Add(item);

            item = new MenuItem() { Id = "Admin", Text = "Admin" };
            item.Items = new List<MenuItem>
            {
                new () { Id = "NoteFiles", Text = "NoteFiles" },
                new () { Id = "Roles", Text = "Roles" },
                new () { Id = "Linked", Text = "Linked" },
                new () { Id = "Hangfire", Text = "Hangfire" }
            };

            menuItemsTop.Add(item);

            // remove what does not apply to this user
            if (!isAdmin)
            {
                menuItemsTop.RemoveAt(3);
            }
            if (isUser || isAdmin)
            {
            }
            else
            {
                menuItemsTop.RemoveAt(1);
                menuItemsTop.RemoveAt(0);
            }
        }

        /// <summary>
        /// Invoked when an Item is selected
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public async Task OnSelect(MenuEventArgs<MenuItem> e)
        {
            await ExecMenu(e.Item.Id);
        }

        /// <summary>
        /// This could potentially be called from other places...
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task ExecMenu(string id)
        {
            switch (id)
            {
                case "MainHelp":
                    Navigation.NavigateTo("help");
                    break;
                case "About":
                    Navigation.NavigateTo("about");
                    break;
                case "License":
                    Navigation.NavigateTo("license");
                    break;

                case "Subscriptions":
                    Navigation.NavigateTo("subscribe");
                    break;

                case "MRecent":
                    Navigation.NavigateTo("tracker");
                    break;

                case "Recent":
                    await StartSeq();
                    break;

                case "NoteFiles":
                    Navigation.NavigateTo("admin/notefilelist");
                    break;

                case "Preferences":
                    Navigation.NavigateTo("preferences");
                    break;

                case "Hangfire":
                    Navigation.NavigateTo(Globals.EditUserVModel.HangfireLoc, true);
                    break;

                case "Roles":
                    Navigation.NavigateTo("admin/editroles");
                    break;

                case "Linked":
                    Navigation.NavigateTo("admin/linkindex");
                    break;
            }
        }

        /// <summary>
        /// Recent menu item - start sequencing
        /// </summary>
        /// <returns></returns>
        private async Task StartSeq()
        {
            // get users list of files
            List<Sequencer> sequencers = await DAL.GetSequencer(Http);
            if (sequencers.Count == 0)
                return;

            // order them as prefered by user
            sequencers = sequencers.OrderBy(p => p.Ordinal).ToList();

            // set up state for sequencing
            await sessionStorage.SetItemAsync<List<Sequencer>>("SeqList", sequencers);
            await sessionStorage.SetItemAsync<int>("SeqIndex", 0);
            await sessionStorage.SetItemAsync<Sequencer>("SeqItem", sequencers[0]);
            await sessionStorage.SetItemAsync<bool>("IsSeq", true); // flag for noteindex
            // begin
            Navigation.NavigateTo("noteindex/" + sequencers[0].NoteFileId);
        }

        /// <summary>
        /// Show error message
        /// </summary>
        /// <param name="message"></param>
        private void ShowMessage(string message)
        {
            var parameters = new ModalParameters();
            parameters.Add("MessageInput", message);
            Modal.Show<MessageBox>("Error", parameters);
        }
    }
}
