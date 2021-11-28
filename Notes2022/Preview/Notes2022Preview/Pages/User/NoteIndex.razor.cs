using Microsoft.AspNetCore.Components;

namespace Notes2022Preview.Pages.User
{
    public partial class NoteIndex
    {
        [Parameter] public int NotesfileId { get; set; }
    }
}
