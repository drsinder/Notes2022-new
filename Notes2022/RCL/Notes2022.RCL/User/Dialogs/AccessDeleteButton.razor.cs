using Microsoft.AspNetCore.Components;
using Notes2022.Shared;

namespace Notes2022.RCL.User.Dialogs
{
    public partial class AccessDeleteButton
    {
        [Parameter]
        public NoteAccess noteAccess { get; set; }

        [Parameter]
        public EventCallback<string> OnClick { get; set; }

        protected async Task Delete()
        {
            await DAL.DeleteAccessItem(Http, noteAccess.NoteFileId, noteAccess.ArchiveId, noteAccess.UserID);
            await OnClick.InvokeAsync("Delete");
        }
    }
}