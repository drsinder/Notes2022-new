/*--------------------------------------------------------------------------
    **
    **Copyright © 2022, Dale Sinder
    **
    **  Description:
    **     Menu bar for Main List of notes
    **
    ** This program is free software: you can redistribute it and / or modify
    ** it under the terms of the GNU General Public License version 3 as
    ** published by the Free Software Foundation.
    **
    ** This program is distributed in the hope that it will be useful,
    ** but WITHOUT ANY WARRANTY; without even the implied warranty of
    ** MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
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
using Notes2022.RCL.User.Dialogs;
using Notes2022.Shared;
using Syncfusion.Blazor.Navigations;
using System.Net.Http.Json;
using System.Text;

namespace Notes2022.RCL.User.Menus
{
    public partial class ListMenu
    {
        [CascadingParameter] public IModalService Modal { get; set; }
        [Parameter] public NoteDisplayIndexModel Model { get; set; }

        [Parameter] public NoteIndex Caller { get; set; }

        private static List<MenuItem> menuItems { get; set; }
        protected SfMenu<MenuItem> topMenu { get; set; }

        private bool HamburgerMode { get; set; } = false;

        private bool IsPrinting { get; set; } = false;
        protected string sliderValueText { get; set; }
        protected int baseNotes { get; set; }
        protected int currNote { get; set; }

        [Inject] HttpClient Http { get; set; }
        [Inject] NavigationManager Navigation { get; set; }
        public ListMenu()
        {
        }

        protected override async Task OnParametersSetAsync()
        {
            baseNotes = Model.Notes.Count;
            sliderValueText = "1/" + baseNotes;
            menuItems = new List<MenuItem>();

            MenuItem item = new() { Id = "ListNoteFiles", Text = "List Note Files" };
            menuItems.Add(item);
            if (Model.myAccess.Write)
            {
                item = new() { Id = "NewBaseNote", Text = "New Base Note" };
                menuItems.Add(item);
            }
            if (Model.myAccess.ReadAccess)
            {
                MenuItem item2 = new() { Id = "OutPutMenu", Text = "Output" };
                item2.Items = new List<MenuItem>
                {
                    new () { Id = "eXport", Text = "eXport" },
                    new () { Id = "HtmlFromIndex", Text = "Html (expandable)" },
                    new () { Id = "htmlFromIndex", Text = "html (flat)" },
                    new () { Id = "mailFromIndex", Text = "mail" },
                    //item2.Items.Add(new MenuItem() { Id = "MarkMine", Text = "Mark my notes for output" });
                    new MenuItem() { Id = "PrintFile", Text = "Print entire file" },

                    //if (Model.isMarked)
                    //{
                    //    item2.Items.Add(new MenuItem() { Id = "OutputMarked", Text = "Output marked notes" });
                    //}

                    new (){ Id = "JsonExport", Text = "Json Export" }
                };

                menuItems.Add(item2);

                menuItems.Add(new MenuItem() { Id = "SearchFromIndex", Text = "Search" });
                menuItems.Add(new MenuItem() { Id = "ListHelp", Text = "Z for HELP" });
                if (Model.myAccess.EditAccess || Model.myAccess.ViewAccess)
                {
                    menuItems.Add(new MenuItem() { Id = "AccessControls", Text = "Access Controls" });
                }
            }

            //Width = await jsRuntime.InvokeAsync<int>("getWidth", "x");
        }

        public async Task OnSelect(MenuEventArgs<MenuItem> e)
        {
            await ExecMenu(e.Item.Id);
        }

        public async Task ExecMenu(string id)
        {
            switch (id)
            {
                case "ListNoteFiles":
                    Navigation.NavigateTo("notesfiles/");
                    break;

                case "ReloadIndex": // only a direct type in
                    Navigation.NavigateTo("noteindex/" + Model.noteFile.Id, true);
                    break;

                case "NewBaseNote":
                    Navigation.NavigateTo("newnote/" + Model.noteFile.Id + "/0" + "/0");
                    break;

                case "ListHelp":
                    Modal.Show<HelpDialog>();
                    break;

                case "AccessControls":
                    var parameters = new ModalParameters();
                    parameters.Add("fileId", Model.noteFile.Id);
                    Modal.Show<AccessList>("", parameters);
                    break;

                case "eXport":
                    DoExport(false, false);
                    break;

                case "HtmlFromIndex":
                    DoExport(true, true);
                    break;

                case "htmlFromIndex":
                    DoExport(true, false);
                    break;

                case "JsonExport":
                    DoJson();
                    break;

                case "mailFromIndex":
                    await DoEmail();
                    break;

                case "PrintFile":
                    await PrintFile();
                    break;

                case "SearchFromIndex":
                    await SetSearch();
                    break;

                default:
                    break;

            }
        }

        private async Task SetSearch()
        {
            var parameters = new ModalParameters();
            parameters.Add("searchtype", "File");
            var formModal = Modal.Show<SearchDlg>();
            var result = await formModal.Result;
            if (result.Cancelled)
                return;
            else
            {
                Search target = (Search)result.Data;
                // start the search
                await Caller.StartSearch(target);
                return;
            }
        }

        private async Task PrintFile()
        {
            currNote = 1;
            IsPrinting = true;
            await PrintFile2();
            IsPrinting = false;
        }

        /// <summary>
        /// Print a whole file
        /// </summary>
        protected async Task PrintFile2()
        {
            string respX = String.Empty;

            // keep track of base note
            NoteHeader baseHeader = Model.Notes[0];

            NoteHeader currentHeader = Model.Notes[0];

            StringBuilder sb = new();

            sb.Append("<h4 class=\"text-center\">" + Model.noteFile.NoteFileTitle + "</h4>");

        reloop: // come back here to do another note
            respX = "";
            if (currentHeader.ResponseCount > 0)
                respX = " - " + currentHeader.ResponseCount + " Responses ";
            else if (currentHeader.ResponseOrdinal > 0)
                respX = " Response " + currentHeader.ResponseOrdinal;


            sb.Append("<div class=\"noteheader\"><p> <span class=\"keep-right\">Note: ");
            sb.Append(currentHeader.NoteOrdinal + " " + respX);
            sb.Append("&nbsp;&nbsp;&nbsp;&nbsp;</span></p><h4>Subject: ");
            sb.Append(currentHeader.NoteSubject);
            sb.Append("<br />Author: ");
            sb.Append(currentHeader.AuthorName + "    ");
            sb.Append((Globals.LocalTimeBlazor(currentHeader.LastEdited).ToLongDateString()) + " " 
                + (Globals.LocalTimeBlazor(currentHeader.LastEdited).ToShortTimeString()));

            NoteContent currentContent = await DAL.GetExport2(Http, currentHeader.Id);

            if (!string.IsNullOrEmpty(currentHeader.DirectorMessage))
            {
                sb.Append("<br /><span>Director Message: ");
                sb.Append(currentHeader.DirectorMessage);
                sb.Append("</span>");
            }
            //if (tags is not null && tags.Count > 0)
            //{
            //    sb.Append(" <br /><span>Tags: ");
            //    foreach (Tags tag in tags)
            //    {
            //        sb.Append(tag.Tag + " ");
            //    }
            //    sb.Append("</span>");
            //}
            sb.Append("</h4></div><div class=\"notebody\" >");
            sb.Append(currentContent.NoteBody);
            sb.Append("</div>");

            if (currentHeader.ResponseOrdinal < baseHeader.ResponseCount) // more responses in string
            {
                currentHeader = Model.AllNotes.Single(p => p.NoteOrdinal == currentHeader.NoteOrdinal && p.ResponseOrdinal == currentHeader.ResponseOrdinal + 1);

                goto reloop;        // print another note
            }

            currentHeader = baseHeader; // set back to base note

            NoteHeader next = Model.Notes.SingleOrDefault(p => p.NoteOrdinal == currentHeader.NoteOrdinal + 1);
            if (next is not null)       // still base notes left to print
            {
                currentHeader = next;   // set current note and base note
                baseHeader = next;
                //await SetNote();        // set important stuff
                sliderValueText = currentHeader.NoteOrdinal + "/" + baseNotes;  // update progress test
                currNote = currentHeader.NoteOrdinal;                           // update progress bar

                goto reloop;    // print another string
            }

            string stuff = sb.ToString();           // turn accumulated output into a string

            var parameters = new ModalParameters();
            parameters.Add("PrintStuff", stuff);    // pass string to print dialog
            Modal.Show<PrintDlg>("", parameters);   // invloke print dialog with our output

        }

        private void DoExport(bool isHtml, bool isCollapsible, bool isEmail = false, string emailaddr = null)
        {
            var parameters = new ModalParameters();

            ExportViewModel vm = new();
            vm.ArchiveNumber = Model.ArcId;
            vm.isCollapsible = isCollapsible;
            vm.isDirectOutput = !isEmail;
            vm.isHtml = isHtml;
            vm.NoteFile = Model.noteFile;
            vm.NoteOrdinal = 0;
            vm.Email = emailaddr;

            parameters.Add("Model", vm);
            parameters.Add("FileName", Model.noteFile.NoteFileName + (isHtml ? ".html" : ".txt"));

            Modal.Show<ExportUtil1>("", parameters);
        }

        private void DoJson()
        {
            var parameters = new ModalParameters();

            ExportViewModel vm = new();
            vm.ArchiveNumber = Model.ArcId;
            vm.NoteFile = Model.noteFile;
            vm.NoteOrdinal = 0;

            parameters.Add("model", vm);

            Modal.Show<ExportJson>("", parameters);
        }

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



            //            ShowMessage(emailaddr);

        }


        private void ShowMessage(string message)
        {
            var parameters = new ModalParameters();
            parameters.Add("MessageInput", message);
            Modal.Show<MessageBox>("", parameters);
        }
    }
}
