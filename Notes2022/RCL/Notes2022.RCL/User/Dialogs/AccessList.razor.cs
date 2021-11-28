using Blazored.Modal;
using Blazored.Modal.Services;
//using Grpc.Net.Client;
using Microsoft.AspNetCore.Components;
using Notes2022.Shared;
using Syncfusion.Blazor.Grids;
using System.Net.Http.Json;

namespace Notes2022.RCL.User.Dialogs
{
    public partial class AccessList
    {
        [CascadingParameter] public IModalService Modal { get; set; }
        [CascadingParameter] public BlazoredModalInstance ModalInstance { get; set; }
        [Parameter] public int fileId { get; set; }

        private SfGrid<NoteAccess> MyGrid;
        private List<NoteAccess> myList { get; set; }
        private List<NoteAccess> temp { get; set; }
        private List<UserData> userList { get; set; }
        private NoteAccess myAccess { get; set; }
        private int arcId { get; set; }

        private string message { get; set; }

        [Inject] HttpClient Http { get; set; }
        //[Inject] GrpcChannel Channel { get; set; }
        [Inject] Blazored.SessionStorage.ISessionStorageService sessionStorage { get; set; }
        public AccessList()
        {
        }

        protected async override Task OnParametersSetAsync()
        {
            arcId = await sessionStorage.GetItemAsync<int>("ArcId");

            temp = await DAL.GetAccessList(Http, fileId);
            myList = new List<NoteAccess>();

            foreach (NoteAccess item in temp)
            {
                if (item.ArchiveId == arcId)
                {
                    myList.Add(item);
                }
            }

            userList = await DAL.GetUserList(Http);

            try
            {
                myAccess = await DAL.GetMyAccess(Http, fileId);
            }
            catch (Exception ex)
            {
                message += ex.Message;
                myAccess = new NoteAccess();
            }
        }

        private void Cancel()
        {
            ModalInstance.CancelAsync();
        }

        protected async void CreateNew()
        {
            var parameters = new ModalParameters();
            parameters.Add("userList", userList);
            parameters.Add("NoteFileId", fileId);

            var xx = Modal.Show<AddAccessDlg>("", parameters);
            await xx.Result;

            StateHasChanged();
            MyGrid.Refresh();
        }

        protected async Task ClickHandler(string newMessage)
        {
            arcId = await sessionStorage.GetItemAsync<int>("ArcId");

            temp = await DAL.GetAccessList(Http, fileId);
            myList = new List<NoteAccess>();

            foreach (NoteAccess item in temp)
            {
                if (item.ArchiveId == arcId)
                {
                    myList.Add(item);
                }
            }
            StateHasChanged();
            MyGrid.Refresh();
        }

    }
}
