using Microsoft.AspNetCore.Components;

namespace Notes2022Preview.Pages.User
{
    public partial class EditNote
    {
        [Parameter] public long NoteId { get; set; }   //  what we are editing

    }
}
