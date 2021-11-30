using Microsoft.AspNetCore.Components;
using Notes2022.Shared;
using System.Net.Http.Json;

namespace Notes2022.RCL.User
{
    /// <summary>
    /// Setup for calling note editor panel to edit an existing note
    /// </summary>
    public partial class EditNote
    {
        [Parameter] public long NoteId { get; set; }   //  what we are editing

        /// <summary>
        /// our data for the note in edit model
        /// </summary>
        protected TextViewModel Model { get; set; } = new TextViewModel();

        /// <summary>
        /// A note display model
        /// </summary>
        protected DisplayModel stuff { get; set; }

        [Inject] HttpClient Http { get; set; }
        public EditNote()
        {
        }

        // get all the data
        protected override async Task OnParametersSetAsync()
        {
            stuff = await DAL.GetNoteContent(Http, NoteId);

            Model.NoteFileID = stuff.noteFile.Id;
            Model.NoteID = NoteId;
            Model.BaseNoteHeaderID = stuff.header.BaseNoteId;
            Model.RefId = stuff.header.RefId;
            Model.MyNote = stuff.content.NoteBody;
            Model.MySubject = stuff.header.NoteSubject;
            Model.DirectorMessage = stuff.header.DirectorMessage;

            string tags = "";
            foreach (var tag in stuff.tags)
            {
                tags += tag + " ";
            }
            Model.TagLine = tags;
        }
    }
}
