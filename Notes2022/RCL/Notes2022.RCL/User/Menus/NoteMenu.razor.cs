/*--------------------------------------------------------------------------
    **
    **Copyright © 2022, Dale Sinder
    **
    **  Name: NoteMenu.razor.cs
    **
    ** Description:
    ** Menu bar for Note display
    **
    **  This program is free software: you can redistribute it and / or modify
    **  it under the terms of the GNU General Public License version 3 as
    **  published by the Free Software Foundation.
    **
    **  This program is distributed in the hope that it will be useful,
    **  but WITHOUT ANY WARRANTY; without even the implied warranty of
    **  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    **  GNU General Public License version 3 for more details.
    **
    **  You should have received a copy of the GNU General Public License
    **  version 3 along with this program in file "license-gpl-3.0.txt".
    **  If not, see <http://www.gnu.org/licenses/gpl-3.0.txt>.
    **
    **--------------------------------------------------------------------------*/

using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Notes2022.RCL.User.Dialogs;
using Notes2022.Shared;
using Syncfusion.Blazor.Navigations;
using System.Net.Http.Json;

namespace Notes2022.RCL.User.Menus
{
    /// <summary>
    /// Menu for top of a Note
    /// </summary>
    public partial class NoteMenu
    {
        /// <summary>
        /// For dialogs
        /// </summary>
        [CascadingParameter] public IModalService Modal { get; set; }

        /// <summary>
        /// Model data reference from container
        /// </summary>
        [Parameter] public DisplayModel Model { get; set; }

        /// <summary>
        /// Reference to our caller/container so we can call back into it.
        /// </summary>
        [Parameter] public NoteIndex MyNoteIndex { get; set; }

        /// <summary>
        /// Menu structure
        /// </summary>
        private static List<MenuItem> menuItems { get; set; }

        /// <summary>
        /// Top level menu instance
        /// </summary>
        protected SfMenu<MenuItem> topMenu { get; set; }

        private bool HamburgerMode { get; set; } = false;

        [Inject] HttpClient Http { get; set; }
        [Inject] NavigationManager Navigation { get; set; }
        public NoteMenu()
        {
        }

        /// <summary>
        /// Construct our menu based on user access token
        /// </summary>
        /// <returns></returns>
        protected override Task OnParametersSetAsync()
        {
            menuItems = new List<MenuItem>();

            MenuItem item = new() { Id = "ListNotes", Text = "Listing" };
            menuItems.Add(item);

            item = new() { Id = "NextBase", Text = "Next Base" };
            menuItems.Add(item);

            item = new() { Id = "PreviousBase", Text = "Previous Base" };
            menuItems.Add(item);

            item = new() { Id = "NextNote", Text = "Next" };
            menuItems.Add(item);

            item = new() { Id = "PreviousNote", Text = "Previous" };
            menuItems.Add(item);

            if (Model.access.ReadAccess)
            {
                MenuItem item2 = new() { Id = "OutPutMenu", Text = "Output" };
                item2.Items = new List<MenuItem>
                {
                    new() { Id = "Forward", Text = "Forward" },
                    new() { Id = "Copy", Text = "Copy" },
                    new() { Id = "mail", Text = "mail" },
                    //item2.Items.Add(new MenuItem() { Id = "Mark", Text = "Mark for output" });
                    new() { Id = "Html", Text = "Html (expandable)" },
                    new() { Id = "html", Text = "html (flat)" }
                };
                menuItems.Add(item2);

                if (Model.access.Respond)
                {
                    item = new() { Id = "NewResponse", Text = "New Response" };
                    menuItems.Add(item);
                }

                if (Model.CanEdit)
                {
                    item = new() { Id = "Edit", Text = "Edit" };
                    menuItems.Add(item);

                    if (Model.access.UserID == Model.header.AuthorID || Model.IsAdmin)
                    {
                        item = new() { Id = "Delete", Text = "Delete" };
                        menuItems.Add(item);
                    }
                }

                menuItems.Add(new() { Id = "SearchFromNote", Text = "Search" });
                menuItems.Add(new() { Id = "NoteHelp", Text = "Z for HELP" });
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Menu item invoked
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public async Task OnSelect(MenuEventArgs<MenuItem> e)
        {
            await ExecMenu(e.Item.Id);
        }

        /// <summary>
        /// This can be called not only from above but also by the container
        /// that shares the same model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task ExecMenu(string id)
        {
            long myId;
            switch (id)
            {
                case "ListNotes":
                    MyNoteIndex.Listing();
                    break;

                case "NewResponse":
                    long bnId = Model.header.Id;           // if base note
                    if (Model.header.ResponseOrdinal > 0)   // if response
                    {
                        bnId = Model.header.BaseNoteId;
                    }
                    Navigation.NavigateTo("newnote/" + Model.noteFile.Id + "/" + bnId + "/" + Model.header.Id);
                    break;

                case "Edit":
                    if (Model.CanEdit)
                        Navigation.NavigateTo("editnote/" + Model.header.Id, true);
                    break;

                case "NextBase":
                    myId = MyNoteIndex.GetNextBaseNote(Model.header);
                    if (myId > 0)
                    {
                        MyNoteIndex.GotoNote(myId);
                    }
                    break;

                case "PreviousBase":
                    myId = MyNoteIndex.GetPreviousBaseNote(Model.header);

                    if (myId > 0)
                    {
                        MyNoteIndex.GotoNote(myId);
                    }
                    break;

                case "NextNote":
                    myId = MyNoteIndex.GetNextNote(Model.header);
                    if (myId > 0)
                    {
                        MyNoteIndex.GotoNote(myId);
                    }
                    break;

                case "PreviousNote":
                    myId = MyNoteIndex.GetPreviousNote(Model.header);

                    if (myId > 0)
                    {
                        MyNoteIndex.GotoNote(myId);
                    }
                    break;

                case "NoteHelp":
                    Modal.Show<HelpDialog2>();
                    break;

                case "Delete":
                    if (Model.IsAdmin || Model.CanEdit)
                    {
                        if (!await YesNo("Are you sure you want to delete this note?"))
                            return;
                        await DAL.DeleteNote(Http, Model.header.Id);
                        Navigation.NavigateTo("notedisplay/" + Model.header.Id, true);
                    }
                    else
                    {
                        ShowMessage("You may not delete this note.");
                    }

                    break;

                case "Forward":
                    Forward();
                    break;

                case "Copy":
                    var parameters = new ModalParameters();
                    parameters.Add("Note", Model.header);
                    Modal.Show<Copy>("", parameters);
                    break;

                case "mail":
                    await DoEmail();
                    break;

                case "Html":
                    DoExport(true, true);
                    break;

                case "html":
                    DoExport(true, false);
                    break;

            }
        }

        /// <summary>
        /// Forwards a note
        /// </summary>
        protected void Forward()
        {
            var parameters = new ModalParameters();
            ForwardViewModel fv = new();
            fv.NoteID = Model.header.Id;
            fv.FileID = Model.header.NoteFileId;
            fv.ArcID = Model.header.ArchiveId;
            fv.NoteOrdinal = Model.header.NoteOrdinal;
            fv.NoteSubject = Model.header.NoteSubject;
            fv.NoteFile = Model.noteFile;

            if (Model.header.ResponseCount > 0 || Model.header.BaseNoteId > 0)
                fv.hasstring = true;

            parameters.Add("ForwardView", fv);

            Modal.Show<Forward>("", parameters);
        }

        /// <summary>
        /// Exports a note
        /// </summary>
        /// <param name="isHtml"></param>
        /// <param name="isCollapsible"></param>
        /// <param name="isEmail"></param>
        /// <param name="emailaddr"></param>
        private void DoExport(bool isHtml, bool isCollapsible, bool isEmail = false, string emailaddr = null)
        {
            var parameters = new ModalParameters();

            ExportViewModel vm = new();
            vm.ArchiveNumber = Model.header.ArchiveId;
            vm.isCollapsible = isCollapsible;
            vm.isDirectOutput = !isEmail;
            vm.isHtml = isHtml;
            vm.NoteFile = Model.noteFile;
            vm.NoteOrdinal = Model.header.NoteOrdinal;
            vm.Email = emailaddr;

            parameters.Add("Model", vm);
            parameters.Add("FileName", Model.noteFile.NoteFileName + (isHtml ? ".html" : ".txt"));

            Modal.Show<ExportUtil1>("", parameters);
        }

        /// <summary>
        /// Emails a note
        /// </summary>
        /// <returns></returns>
        private async Task DoEmail()
        {
            string emailaddr;
            var parameters = new ModalParameters();
            var formModal = Modal.Show<Email>("", parameters);
            var result = await formModal.Result;
            if (result.Cancelled)
                return;
            emailaddr = (string)result.Data;
            if (string.IsNullOrEmpty(emailaddr))
                return;

            DoExport(true, true, true, emailaddr);

        }

        /// <summary>
        /// Confirmation dialog
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task<bool> YesNo(string message)
        {
            var parameters = new ModalParameters();
            parameters.Add("MessageInput", message);
            var formModal = Modal.Show<YesNo>("", parameters);
            var result = await formModal.Result;
            return !result.Cancelled;
        }

        private void ShowMessage(string message)
        {
            var parameters = new ModalParameters();
            parameters.Add("MessageInput", message);
            Modal.Show<MessageBox>("", parameters);
        }
    }
}
