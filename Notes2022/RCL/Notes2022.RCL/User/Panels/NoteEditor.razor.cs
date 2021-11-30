/*--------------------------------------------------------------------------
    **
    **Copyright © 2022, Dale Sinder
    **
    **  Name: NoteEditor.razor.cs
    **
    ** Description: Edits and creates new notes
    **
    **
    ** This program is free software: you can redistribute it and/or modify
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
using Microsoft.AspNetCore.Components.Web;
using Notes2022.RCL.User.Dialogs;
using Notes2022.Shared;
using Syncfusion.Blazor.RichTextEditor;
using System.Net.Http.Json;

namespace Notes2022.RCL.User.Panels
{
    /// <summary>
    /// Editor for notes
    /// Has a fields for:
    /// Subject, Tags, Director Message, and the Note Body
    /// </summary>
    public partial class NoteEditor
    {
        /// <summary>
        /// For dialogs
        /// </summary>
        [CascadingParameter] public IModalService Modal { get; set; }

        /// <summary>
        /// The data model used
        /// </summary>
        [Parameter] public TextViewModel Model { get; set; }

        private bool ShowChild = false;

        /// <summary>
        /// The Notefile we are using
        /// </summary>
        private NoteFile noteFile { get; set; } = new NoteFile();

        /// <summary>
        /// Reference to the Body Editor
        /// </summary>
        private SfRichTextEditor EditObj { get; set; }

        /// <summary>
        /// Reference to the Editor Tool bar
        /// </summary>
        private RichTextEditorToolbarSettings ToolBarObj { get; set; }

        /// <summary>
        /// Holding place to prepared code to be inserted
        /// </summary>
        protected string PreparedCode { get; set; }

        /// <summary>
        /// Define the content of the toolbar
        /// </summary>
        private List<ToolbarItemModel> Tools = new List<ToolbarItemModel>()
        {
            new ToolbarItemModel() { Command = ToolbarCommand.Undo },
            new ToolbarItemModel() { Command = ToolbarCommand.Redo },
            new ToolbarItemModel() { Command = ToolbarCommand.Separator },
            new ToolbarItemModel() { Command = ToolbarCommand.Bold },
            new ToolbarItemModel() { Command = ToolbarCommand.Italic },
            new ToolbarItemModel() { Command = ToolbarCommand.Underline },
            new ToolbarItemModel() { Command = ToolbarCommand.StrikeThrough },
            new ToolbarItemModel() { Command = ToolbarCommand.FontName },
            new ToolbarItemModel() { Command = ToolbarCommand.FontSize },
            new ToolbarItemModel() { Command = ToolbarCommand.FontColor },
            new ToolbarItemModel() { Command = ToolbarCommand.BackgroundColor },
            new ToolbarItemModel() { Command = ToolbarCommand.LowerCase },
            new ToolbarItemModel() { Command = ToolbarCommand.UpperCase },
            new ToolbarItemModel() { Command = ToolbarCommand.Separator },
            new ToolbarItemModel() { Command = ToolbarCommand.Formats },
            new ToolbarItemModel() { Command = ToolbarCommand.Alignments },
            new ToolbarItemModel() { Command = ToolbarCommand.OrderedList },
            new ToolbarItemModel() { Command = ToolbarCommand.UnorderedList },
            new ToolbarItemModel() { Command = ToolbarCommand.Outdent },
            new ToolbarItemModel() { Command = ToolbarCommand.Indent },
            new ToolbarItemModel() { Command = ToolbarCommand.Separator },
            new ToolbarItemModel() { Command = ToolbarCommand.CreateLink },
            new ToolbarItemModel() { Command = ToolbarCommand.Image },
            new ToolbarItemModel() { Command = ToolbarCommand.CreateTable },
            new ToolbarItemModel() { Command = ToolbarCommand.Separator },
            new ToolbarItemModel() { Command = ToolbarCommand.ClearFormat },
            new ToolbarItemModel() { Command = ToolbarCommand.Print },
            //new ToolbarItemModel() { Command = ToolbarCommand.InsertCode },
            new ToolbarItemModel() { Name = "PCode1", TooltipText = "Prepare Code for Insertion" },
            new ToolbarItemModel() { Name = "PCode", TooltipText = "Insert Prepared Code" },
            new ToolbarItemModel() { Command = ToolbarCommand.SourceCode },
            new ToolbarItemModel() { Command = ToolbarCommand.FullScreen }
        };

        [Inject] HttpClient Http { get; set; }
        [Inject] NavigationManager Navigation { get; set; }
        public NoteEditor()
        {
        }

        /// <summary>
        /// Get a NoteFile Object for the file we are using
        /// </summary>
        /// <returns></returns>
        protected async override Task OnParametersSetAsync()
        {
            if (Model.NoteFileID != 0)
                noteFile = await DAL.GetNewNote(Http, Model.NoteFileID);
        }

        /// <summary>
        /// User has asked to store the note
        /// </summary>
        /// <returns></returns>
        protected async Task HandleValidSubmit()
        {
            if (string.IsNullOrEmpty(Model.MySubject))
            {
                ShowMessage("Please provide a note Subject");
                return;
            }

            if (Model.NoteID == 0)    // new note
            {
                await DAL.CreateNewNote(Http, Model);
            }
            else // editing
            {
                await DAL.UpdateNote(Http, Model);
            }
            NoteHeader nh2 = await DAL.GetNewNote2(Http);
            Navigation.NavigateTo("notedisplay/" + nh2.Id);
            return;

        }

        //public async Task OnToolbarClickHandler(ToolbarClickEventArgs args)
        //{
        //    if (args.Item.Id == "InsertCode")
        //    {
        //        string xx = await EditObj.GetSelectedHtmlAsync();
        //        if (xx is not null && xx.Length > 0)
        //        {
        //            ShowMessage("Code can not be edited.  Please Copy, Delete, and Reinsert");
        //            return;
        //        }
        //        // get insertion point?? how??

        //        var parameters = new ModalParameters();
        //        parameters.Add("stuff", xx);
        //        parameters.Add("EditObj", EditObj);
        //        var formModal = Modal.Show<CodeFormat>("", parameters);
        //        var result = await formModal.Result;
        //        if (!result.Cancelled)
        //        {
        //            PreparedCode = (string)result.Data;
        //        }
        //    }
        //}

        /// <summary>
        /// Prepare code for insertion - collect the text
        /// </summary>
        /// <returns></returns>
        public async Task InsertCode1()
        {
            string xx = await EditObj.GetSelectedHtmlAsync();
            if (xx is not null && xx.Length > 0)
            {
                ShowMessage("Code can not be edited.  Please Copy, Delete, and Reinsert");
                return;
            }
            // get insertion point?? how??

            var parameters = new ModalParameters();
            parameters.Add("stuff", xx);
            parameters.Add("EditObj", EditObj);
            var formModal = Modal.Show<CodeFormat>("", parameters);
            var result = await formModal.Result;
            if (!result.Cancelled)
            {
                PreparedCode = (string)result.Data;
            }
        }

        /// <summary>
        /// Insert the previously prepared code.
        /// </summary>
        /// <returns></returns>
        public async Task InsertCode2()
        {
            if (!string.IsNullOrEmpty(PreparedCode))
                await EditObj.ExecuteCommandAsync(CommandName.InsertHTML, PreparedCode);
        }

        /// <summary>
        /// Shows a message
        /// </summary>
        /// <param name="message"></param>
        private void ShowMessage(string message)
        {
            var parameters = new ModalParameters();
            parameters.Add("MessageInput", message);
            Modal.Show<MessageBox>("", parameters);
        }

        /// <summary>
        /// Cancel out of editing
        /// </summary>
        protected void CancelEdit()
        {
            Navigation.NavigateTo("noteindex/" + Model.NoteFileID);
        }

        //protected void OnClickRef(MouseEventArgs args)
        //{
        //    ShowChild = true;
        //}

        //private void OnClickRefHide(MouseEventArgs args)
        //{
        //    ShowChild = false;
        //}
    }
}
