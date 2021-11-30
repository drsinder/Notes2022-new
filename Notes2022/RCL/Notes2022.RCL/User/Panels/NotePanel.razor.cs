/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: NotePanel.razor.cs
    **
    ** Description:
    **      Displays a note - may be used recursively
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
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Notes2022.RCL.User.Dialogs;
using Notes2022.RCL.User.Menus;
using Notes2022.Shared;
using Syncfusion.Blazor.Inputs;
using System.Net.Http.Json;
using System.Text;
using System.Timers;

namespace Notes2022.RCL.User.Panels
{
    /// <summary>
    /// This panel is an optionally hidden panel that resides inside the note index.
    /// The index replaces itself with this panel when the user selects a note to
    /// view.  This allows the index and note panel to share data and communicate
    /// with each other more readily.
    /// </summary>
    public partial class NotePanel
    {
        /// <summary>
        /// For Dialogs
        /// </summary>
        [CascadingParameter] public IModalService Modal { get; set; }

        /// <summary>
        /// Our current NoteId
        /// </summary>
        [Parameter] public long NoteId { get; set; }

        /// <summary>
        /// Whether or not child windows should be shown.  These might include
        /// Responses, versions, references.
        /// </summary>
        [Parameter] public bool ShowChild { get; set; }

        /// <summary>
        /// Is this at the "root" of something
        /// </summary>
        [Parameter] public bool IsRootNote { get; set; }

        /// <summary>
        /// Should optional buttons be shown
        /// </summary>
        [Parameter] public bool ShowButtons { get; set; } = true;

        /// <summary>
        /// Is this panel to be shown in the alternate style?
        /// </summary>
        [Parameter] public bool AltStyle { get; set; }

        /// <summary>
        /// Should certain functions be suppressed at head and tail of panel
        /// </summary>
        [Parameter] public bool IsMini { get; set; }

        /// <summary>
        /// Are we showing history versions
        /// </summary>
        [Parameter] public int Vers { get; set; } = 0;

        /// <summary>
        /// Who is my container
        /// </summary>
        [Parameter] public NoteIndex MyNoteIndex { get; set; }

        /// <summary>
        /// List of responses
        /// </summary>
        protected List<NoteHeader> respHeaders { get; set; }

        /// <summary>
        /// Header style string
        /// </summary>
        protected string HeaderStyle { get; set; }

        /// <summary>
        /// Body style string
        /// </summary>
        protected string BodyStyle { get; set; }

        /// <summary>
        /// Are responses shown
        /// </summary>
        protected bool RespShown { get; set; }

        /// <summary>
        /// Is the order of responses flipped?
        /// </summary>
        protected bool RespFlipped { get; set; }

        /// <summary>
        /// Should the typing box "eat" the next enter key?
        /// </summary>
        protected bool EatEnter { get; set; }

        /// <summary>
        /// Are we showing version history?
        /// </summary>
        protected bool ShowVers { get; set; } = false;

        /// <summary>
        /// Are we sequencing?
        /// </summary>
        protected bool IsSeq { get; set; }

        /// <summary>
        /// Data Model for Note display
        /// </summary>
        protected DisplayModel model { get; set; }

        /// <summary>
        /// Reference to our menu so we can talk to it
        /// </summary>
        public NoteMenu MyMenu { get; set; }

        /// <summary>
        /// Reference to our fancy html editor
        /// </summary>
        SfTextBox sfTextBox { get; set; }

        /// <summary>
        /// Accumulator for the typin nav box
        /// </summary>
        public string NavString { get; set; }
        //public string NavCurrentVal { get; set; }

        public string respX { get; set; }
        public string respY { get; set; }


        [Inject] HttpClient Http { get; set; }
        [Inject] NavigationManager Navigation { get; set; }
        [Inject] IJSRuntime JS { get; set; }    // enables calling javascript
        [Inject] Blazored.SessionStorage.ISessionStorageService sessionStorage { get; set; }

        /// <summary>
        /// Initialize defaults for a "root" note - not showing children
        /// </summary>
        public NotePanel()
        {
            ShowChild = false;
            IsRootNote = true;
        }

        /// <summary>
        /// Get our data and set IsSeq flag from state
        /// </summary>
        /// <returns></returns>
        protected override async Task OnParametersSetAsync()
        {
            await GetData();

            IsSeq = await sessionStorage.GetItemAsync<bool>("IsSeq");
        }

        /// <summary>
        /// Get data for note
        /// </summary>
        /// <returns></returns>
        protected async Task GetData()
        {
            RespShown = false;

            //Set style for note and header
            HeaderStyle = "noteheader";
            BodyStyle = "notebody";

            if (AltStyle)
            {
                HeaderStyle += "-alt";
                BodyStyle += "-alt";
            }

            // Get data from the server - just the content -
            // we already have the header in the container (index)
            model = await DAL.GetNoteContent(Http, NoteId, Vers);

            // set text to be displayed re responses
            respX = respY = "";
            if (model.header.ResponseCount > 0)
            {
                respX = " - " + model.header.ResponseCount + " Responses ";
            }
            else if (model.header.ResponseOrdinal > 0)
            {
                respX = " Response " + model.header.ResponseOrdinal;
                respY = "." + model.header.ResponseOrdinal;
            }
        }

        //private void OnClickResp(MouseEventArgs args)
        //{
        //    long bnId = model.header.Id;           // if base note
        //    if (model.header.ResponseOrdinal > 0)   // if response
        //    {
        //        bnId = model.header.BaseNoteId;
        //    }

        //    Navigation.NavigateTo("newnote/" + model.noteFile.Id + "/" + bnId + "/" + model.header.Id);
        //}

        /// <summary>
        /// Handle change of responses shown switch
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private async Task ShowRespChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            if (RespShown)
            {
                // Get response headers from the index
                respHeaders = MyNoteIndex.GetResponseHeaders(model.header.Id);
            }
        }

        /// <summary>
        /// Change the order of shown responses
        /// </summary>
        /// <param name="args"></param>
        private void FlipRespChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            if (RespFlipped)
                respHeaders = respHeaders.OrderByDescending(x => x.ResponseOrdinal).ToList();
            else
                respHeaders = respHeaders.OrderBy(x => x.ResponseOrdinal).ToList();
        }

        /// <summary>
        /// Return to index mode...
        /// </summary>
        /// <param name="args"></param>
        private void OnDone(MouseEventArgs args)
        {
            MyNoteIndex.Listing();
        }

        /// <summary>
        /// Print the note
        /// </summary>
        /// <param name="args"></param>
        private async void OnPrint(MouseEventArgs args)
        {
            await PrintString(false);
        }

        /// <summary>
        /// Print the whole string
        /// </summary>
        /// <param name="args"></param>
        private async void OnPrintString(MouseEventArgs args)
        {
            await PrintString(true);
        }

        /// <summary>
        /// Print a whole string if wholeString is true
        /// </summary>
        protected async Task PrintString(bool wholeString)
        {
            NoteDisplayIndexModel Model = MyNoteIndex.GetModel();
            string respX = string.Empty;

            // keep track of base note
            NoteHeader baseHeader = Model.AllNotes.SingleOrDefault(p => p.Id == model.header.Id);

            NoteHeader currentHeader = baseHeader;

            StringBuilder sb = new StringBuilder();

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
            sb.Append(Globals.LocalTimeBlazor(currentHeader.LastEdited).ToLongDateString() + " " + Globals.LocalTimeBlazor(currentHeader.LastEdited).ToShortTimeString()/* + " " + Model.tZone.Abbreviation*/);

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

            if (wholeString && currentHeader.ResponseOrdinal < baseHeader.ResponseCount) // more responses in string
            {
                currentHeader = Model.AllNotes.Single(p => p.NoteOrdinal == currentHeader.NoteOrdinal && p.ResponseOrdinal == currentHeader.ResponseOrdinal + 1);

                goto reloop;        // print another note
            }

            currentHeader = baseHeader; // set back to base note

            string stuff = sb.ToString();           // turn accumulated output into a string

            var parameters = new ModalParameters();
            parameters.Add("PrintStuff", stuff);    // pass string to print dialog
            Modal.Show<PrintDlg>("", parameters);   // invloke print dialog with our output
        }

        /// <summary>
        /// collect input and clear EatEnter
        /// </summary>
        /// <param name="args"></param>
        private async void NavInputHandler(InputEventArgs args)
        {
            NavString = args.Value;
            await Task.CompletedTask;
            EatEnter = false;
        }

        /// <summary>
        /// Clear the NavString
        /// </summary>
        /// <returns></returns>
        private async Task ClearNav()
        {
            NavString = null;
            await Task.CompletedTask;
        }

        /// <summary>
        /// Protential to do something when a key up event happens
        /// Handle single key press commands right away.  Otherwise
        /// let input accumulate.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private async Task KeyUpHandler(KeyboardEventArgs args)
        {
            switch (NavString)
            {
                case "I":
                case "L":
                    await ClearNav();
                    await MyMenu.ExecMenu("ListNotes");
                    return;

                case "N":
                    await ClearNav();
                    await MyMenu.ExecMenu("NewResponse");
                    return;

                case "Z":
                    await ClearNav();
                    Modal.Show<HelpDialog2>();
                    EatEnter = true;
                    return;

                case "E":
                    await ClearNav();
                    await MyMenu.ExecMenu("Edit");
                    return;

                case "B":
                    await ClearNav();
                    await MyMenu.ExecMenu("PreviousBase");
                    return;

                case "b":
                    await ClearNav();
                    await MyMenu.ExecMenu("PreviousNote");
                    return;

                case "D":
                    await ClearNav();
                    await MyMenu.ExecMenu("Delete");
                    EatEnter = true;
                    return;

                case "F":
                    await ClearNav();
                    await MyMenu.ExecMenu("Forward");
                    return;

                case "C":
                    await ClearNav();
                    await MyMenu.ExecMenu("Copy");
                    return;

                case "m":
                    await ClearNav();
                    await MyMenu.ExecMenu("mail");
                    return;

                case "H":
                    await ClearNav();
                    await MyMenu.ExecMenu("Html");
                    return;

                case "h":
                    await ClearNav();
                    await MyMenu.ExecMenu("html");
                    return;

                case " ":
                    await ClearNav();

                    if (args.ShiftKey)
                    {
                        await NextSearch();
                    }
                    else
                    {
                        await SeqNext();
                    }
                    return;

                default:
                    break;
            }

            if (args.Key == "Enter" && EatEnter) // Ignore this next Enter key!
            {
                EatEnter = false;
                return;
            }

            if (args.Key == "Enter")    // handle enter key
            {
                // lone enter shifted enter key -> Next Base Note
                if (args.ShiftKey && string.IsNullOrEmpty(NavString))
                {
                    await MyMenu.ExecMenu("NextBase");
                    await ClearNav();
                    return;
                }
                // - plus shift enter -> previous base note
                else if (args.ShiftKey && NavString == "-")
                {
                    await MyMenu.ExecMenu("PreviousBase");
                    return;
                }
                // - plus enter -> previous note
                else if (NavString == "-")
                {
                    await MyMenu.ExecMenu("PreviousNote");
                    return;
                }
                // lone enter enter key -> Next Note
                else if (string.IsNullOrEmpty(NavString))
                {
                    await MyMenu.ExecMenu("NextNote");
                    await ClearNav();
                    return;
                }

                // now handle more complex entries
                //
                // cases:
                // #
                // #.#
                // .#
                // +#
                // -#
                // +.#
                // -.#
                // +#.#
                // -#.#
                //
                bool IsPlus = false;
                bool IsMinus = false;
                bool IsRespOnly = false;

                // Ignore ; and spaces
                string stuff = NavString.Replace(";", "").Replace(" ", "");

                // check for modifier prefixes + and -
                if (stuff.StartsWith("+"))
                    IsPlus = true;
                if (stuff.StartsWith("-"))
                    IsMinus = true;

                // now strip the modifiers that we have recorded them
                stuff = stuff.Replace("+", "").Replace("-", "");

                // . implies we are working on response navigation
                if (stuff.StartsWith('.'))
                {
                    await ClearNav();
                    IsRespOnly = true;
                    stuff = stuff.Replace(".", ""); // strip the .
                }

                // parse string for # or #.#
                string[] parts = stuff.Split('.');
                if (parts.Length > 2)
                {
                    ShowMessage("Too many '.'s : " + parts.Length);
                }
                int noteNum;
                if (parts.Length == 1)  // dealing with single number
                {
                    if (!int.TryParse(parts[0], out noteNum))   // try to get value
                    {
                        await ClearNav();
                        EatEnter = true;
                        ShowMessage("Could not parse : " + parts[0]);
                    }
                    else
                    {
                        if (!IsRespOnly) // dealing with notes not responses
                        {
                            if (IsPlus) // add or subtract based on prefix if any
                                noteNum = model.header.NoteOrdinal + noteNum;
                            else if (IsMinus)
                                noteNum = model.header.NoteOrdinal - noteNum;
                        }
                        else // dealing with responses
                        {
                            if (IsPlus)  // add or subtract based on prefix if any
                                noteNum = model.header.ResponseOrdinal + noteNum;
                            else if (IsMinus)
                                noteNum = model.header.ResponseOrdinal - noteNum;

                            // look for target response
                            long headerId2 = MyNoteIndex.GetNoteHeaderId(model.header.NoteOrdinal, noteNum);
                            if (headerId2 != 0) // found it!  Get the data and re-render page
                            {
                                NoteId = headerId2;
                                await GetData();
                                StateHasChanged();
                            }
                            else
                                ShowMessage("Could not find note : " + NavString);
                            return;
                        }
                        // look for note
                        long headerId = MyNoteIndex.GetNoteHeaderId(noteNum, 0);
                        if (headerId != 0) // found it!  Get the data and re-render page
                        {
                            NoteId = headerId;
                            await GetData();
                            StateHasChanged();
                        }
                        else
                            ShowMessage("Could not find note : " + NavString);
                        await ClearNav();
                        return;
                    }
                }
                else if (parts.Length == 2)     // #.# entered - direct nav
                {
                    if (!int.TryParse(parts[0], out noteNum))
                    {
                        ShowMessage("Could not parse : " + parts[0]);
                        EatEnter = true;
                    }
                    int noteRespOrd;
                    if (!int.TryParse(parts[1], out noteRespOrd))
                    {
                        ShowMessage("Could not parse : " + parts[1]);
                        EatEnter = true;
                    }
                    if (noteNum != 0 && noteRespOrd != 0)
                    {
                        {
                            if (IsPlus)     // relative nav forward
                                noteNum = model.header.NoteOrdinal + noteNum;
                            else if (IsMinus) // relative nav backward
                                noteNum = model.header.NoteOrdinal - noteNum;
                            // attempt to get note
                            long headerId2 = MyNoteIndex.GetNoteHeaderId(noteNum, 0);
                            if (headerId2 != 0) // found it!  Get the data and re-render page
                            {
                                NoteId = headerId2;
                                await GetData();
                                StateHasChanged();
                            }
                            else
                                ShowMessage("Could not find note : " + NavString);
                        }
                        long headerId = MyNoteIndex.GetNoteHeaderId(noteNum, noteRespOrd);
                        if (headerId != 0)
                        {
                            NoteId = headerId;
                            await GetData();
                            StateHasChanged();
                        }
                        else
                            ShowMessage("Could not find note : " + NavString);
                    }
                    await ClearNav();
                }
            }
        }

        /// <summary>
        /// GET next matching item from search
        /// </summary>
        /// <returns></returns>
        protected async Task NextSearch()
        {
            bool InSearch = await sessionStorage.GetItemAsync<bool>("InSearch");
            if (!InSearch)
                return;

            int SearchIndex = await sessionStorage.GetItemAsync<int>("SearchIndex");
            List<NoteHeader> SearchList = await sessionStorage.GetItemAsync<List<NoteHeader>>("SearchList");

            if ((++SearchIndex + 1) < SearchList.Count)
            {
                long mode = SearchList[SearchIndex].Id;
                await sessionStorage.SetItemAsync<int>("SearchIndex", SearchIndex);

                NoteId = mode;
                await GetData();
                StateHasChanged();
            }
            else
            {
                await sessionStorage.SetItemAsync<bool>("InSearch", false);
                await sessionStorage.RemoveItemAsync("SearchIndex");
                await sessionStorage.RemoveItemAsync("SearchList");

                ShowMessage("Search Completed!");
            }
        }

        /// <summary>
        /// Find next recent note
        /// </summary>
        /// <returns></returns>
        protected async Task SeqNext()
        {
            if (!IsSeq)
                return;

            int currentIndex = await sessionStorage.GetItemAsync<int>("SeqHeaderIndex");
            List<NoteHeader> headerList = await sessionStorage.GetItemAsync<List<NoteHeader>>("SeqHeaders");
            if (headerList.Count > ++currentIndex) // proceed to next note in file
            {
                await sessionStorage.SetItemAsync("SeqHeaderIndex", currentIndex);

                NoteHeader currHeader = headerList[currentIndex];

                await sessionStorage.SetItemAsync("CurrentSeqHeader", currHeader);

                NoteId = currHeader.Id;
                await GetData();
                StateHasChanged();
            }
            else
            {
                // update seq entry for user
                Sequencer seq = await sessionStorage.GetItemAsync<Sequencer>("SeqItem");
                seq.Active = false;
                await DAL.UpateSequencer(Http, seq);

                // goto next file
                List<Sequencer> sequencers = await sessionStorage.GetItemAsync<List<Sequencer>>("SeqList");
                int seqIndex = await sessionStorage.GetItemAsync<int>("SeqIndex");
                if (sequencers.Count <= ++seqIndex)
                {
                    await sessionStorage.SetItemAsync("IsSeq", false);
                    await sessionStorage.RemoveItemAsync("SeqList");
                    await sessionStorage.RemoveItemAsync("SeqItem");
                    await sessionStorage.RemoveItemAsync("SeqIndex");

                    await sessionStorage.RemoveItemAsync("SeqHeaders");
                    await sessionStorage.RemoveItemAsync("SeqHeaderIndex");
                    await sessionStorage.RemoveItemAsync("CurrentSeqHeader");

                    ShowMessage("You have seen all the new notes!");

                    return;  // end it all
                }

                Sequencer currSeq = sequencers[seqIndex];

                await sessionStorage.SetItemAsync("SeqItem", currSeq);

                await sessionStorage.SetItemAsync("SeqIndex", seqIndex);

                Navigation.NavigateTo("noteindex/" + -currSeq.NoteFileId);
            }
        }

        private void ShowMessage(string message)
        {
            var parameters = new ModalParameters();
            parameters.Add("MessageInput", message);
            Modal.Show<MessageBox>("", parameters);
        }

        private IJSObjectReference? module;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)    // load the prism script
            {
                module = await JS.InvokeAsync<IJSObjectReference>("import",
                    "./prism.min.js");
            }
            else
            {
                // have to wait a bit before putting focus in textbox
                if (sfTextBox is not null)
                {
                    //await Task.Delay(300);
                    await sfTextBox.FocusAsync();
                }
                if (module is not null) // highight inserted code with prism script
                {
                    await module.InvokeVoidAsync("doPrism", "x");
                }
            }
        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (module is not null)
            {
                await module.DisposeAsync();
            }
        }
    }
}
