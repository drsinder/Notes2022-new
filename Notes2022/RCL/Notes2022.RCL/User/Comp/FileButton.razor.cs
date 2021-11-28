using Microsoft.AspNetCore.Components;
using Notes2022.Shared;

namespace Notes2022.RCL.User.Comp
{
    public partial class FileButton
    {
        [Parameter] public NoteFile NoteFile { get; set; }

        [Inject] NavigationManager Navigation { get; set; }
        public FileButton()
        {
        }

        protected void OnClick()
        {
            Navigation.NavigateTo("noteindex/" + NoteFile.Id);
        }
    }
}
