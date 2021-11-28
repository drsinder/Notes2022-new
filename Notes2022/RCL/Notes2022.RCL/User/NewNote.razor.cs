using Microsoft.AspNetCore.Components;
using Notes2022.Shared;

namespace Notes2022.RCL.User
{
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

        protected override async Task OnParametersSetAsync()
        {
            Model.NoteFileID = NotesfileId;
            Model.NoteID = 0;
            Model.BaseNoteHeaderID = BaseNoteHeaderId;
            Model.RefId = RefId;
            Model.MyNote = "";
            Model.MySubject = "";
            Model.TagLine = "";
            Model.DirectorMessage = "";
        }
    }
}
