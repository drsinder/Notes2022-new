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
using Notes2022.RCL;
using Notes2022.RCL.User.Dialogs;
using Notes2022.Shared;
using Syncfusion.Blazor.Navigations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Notes2022Preview.Shared
{
    public partial class NavMenuY
    {
        [CascadingParameter] public IModalService Modal { get; set; }

        protected static List<MenuItem> menuItemsTop { get; set; }
        protected SfMenu<MenuItem> topMenu { get; set; }

        private bool collapseNavMenu = true;

        //private string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        [Inject] HttpClient Http { get; set; }
        [Inject] AuthenticationStateProvider AuthProv { get; set; }
        [Inject] NavigationManager Navigation { get; set; }
        [Inject] Blazored.SessionStorage.ISessionStorageService sessionStorage { get; set; }
        public NavMenuY()
        {
        }

        private void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
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
            AuthenticationState authstate = await AuthProv.GetAuthenticationStateAsync();
            if (authstate.User.Identity.IsAuthenticated)
            {
                try
                {
                    // session...

                    //Globals.EditUserVModel = await sessionStorage.GetItemAsync<EditUserViewModel>("EditUserView");

                    if (Globals.EditUserVModel == null)
                    {
                        UserData udata = await DAL.GetUserData(Http);
                        string uid = udata.UserId;
                        Globals.UserData = udata;

                        Globals.EditUserVModel = await DAL.GetUserEdit(Http, uid);
                        //await sessionStorage.SetItemAsync("EditUserView", Globals.EditUserVModel);

                    }

                    if (Globals.RolesValid)
                        goto Found;

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

        public async Task OnSelect(MenuEventArgs<MenuItem> e)
        {
            await ExecMenu(e.Item.Id);
        }

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

        private async Task StartSeq()
        {
            List<Sequencer> sequencers = await DAL.GetSequencer(Http);
            if (sequencers.Count == 0)
                return;

            sequencers = sequencers.OrderBy(p => p.NoteFileId).ToList();

            await sessionStorage.SetItemAsync<List<Sequencer>>("SeqList", sequencers);
            await sessionStorage.SetItemAsync<int>("SeqIndex", 0);
            await sessionStorage.SetItemAsync<Sequencer>("SeqItem", sequencers[0]);
            await sessionStorage.SetItemAsync<bool>("IsSeq", true);
            Navigation.NavigateTo("noteindex/" + sequencers[0].NoteFileId);
        }

        private void ShowMessage(string message)
        {
            var parameters = new ModalParameters();
            parameters.Add("MessageInput", message);
            Modal.Show<MessageBox>("Error", parameters);
        }
    }
}
