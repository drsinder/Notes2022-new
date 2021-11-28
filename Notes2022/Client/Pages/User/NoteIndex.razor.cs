using Microsoft.AspNetCore.Components;

namespace Notes2022.Client.Pages.User
{
    public partial class NoteIndex
    {
        [Parameter] public int NotesfileId { get; set; }
    }
}
