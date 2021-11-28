using Microsoft.AspNetCore.Components;

namespace Notes2022Preview.Pages.User
{
    public partial class NewNote
    {
        [Parameter] public int NotesfileId { get; set; }
        [Parameter] public long BaseNoteHeaderId { get; set; }   //  base note we are responding to
        [Parameter] public long RefId { get; set; }   //  what we are responding to

    }
}
