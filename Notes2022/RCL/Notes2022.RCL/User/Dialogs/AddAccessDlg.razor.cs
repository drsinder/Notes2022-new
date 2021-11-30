using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Notes2022.Shared;
using System.Net.Http.Json;
using System.Timers;


namespace Notes2022.RCL.User.Dialogs
{
    /// <summary>
    /// Add a user to the access list
    /// </summary>
    public partial class AddAccessDlg
    {
        [CascadingParameter] public BlazoredModalInstance ModalInstance { get; set; }
        [Parameter] public List<UserData> userList { get; set; }
        [Parameter] public int NoteFileId { get; set; }

        protected string selectedUserId { get; set; }

        protected System.Timers.Timer delay { get; set; }

        protected string ArcString { get; set; }

        [Inject] HttpClient Http { get; set; }
        [Inject] Blazored.SessionStorage.ISessionStorageService sessionStorage { get; set; }
        public AddAccessDlg()
        {
        }

        protected override void OnParametersSet()
        {
            selectedUserId = "none";
            //if (NoteFile is null)
            //    Cancel();
        }

        private void Cancel()
        {
            ModalInstance.CancelAsync();
        }

        private async Task Create()
        {
            if (selectedUserId != "none")
            {
                int aId = await sessionStorage.GetItemAsync<int>("ArcId");

                NoteAccess item = new();

                item.UserID = selectedUserId;
                item.NoteFileId = NoteFileId;
                item.ArchiveId = aId;
                // all access options left false

                await DAL.CreateAccessItem(Http, item);

                delay = new(250);       // allow time for server to respond
                delay.Enabled = true;
                delay.Elapsed += Done;  // then close the dialog automatically

                return;
            }

            await ModalInstance.CancelAsync();
        }

        public void Done(Object source, ElapsedEventArgs e)
        {
            delay.Enabled = false;
            delay.Stop();
            ModalInstance.CancelAsync();
        }


    }
}
