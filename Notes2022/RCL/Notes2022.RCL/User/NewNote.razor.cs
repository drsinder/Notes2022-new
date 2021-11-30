using Microsoft.AspNetCore.Components;
using Notes2022.Shared;

namespace Notes2022.RCL.User
{
    /// <summary>
    /// FOr creating a new note
    /// </summary>
    public partial class NewNote
    {
        [Parameter] public int NotesfileId { get; set; }
        [Parameter] public long BaseNoteHeaderId { get; set; }   //  base note we are responding to
        [Parameter] public long RefId { get; set; }   //  what we are responding to

        protected TextViewModel Model { get; set; } = new TextViewModel();

        [Inject] HttpClient Http { get; set; }
        public NewNote()
        {
        }

        /// <summary>
        /// Just fill in a few fields and we are ready...
        /// </summary>
        /// <returns></returns>
        protected override async Task OnParametersSetAsync()
        {
            Model.NoteFileID = NotesfileId; // which file?
            Model.NoteID = 0;               // 0 for new note
            Model.BaseNoteHeaderID = BaseNoteHeaderId;  // base note we are responding to
            Model.RefId = RefId;            // note we are responding to
            Model.MyNote = "";
            Model.MySubject = "";
            Model.TagLine = "";
            Model.DirectorMessage = "";
        }
    }
}
