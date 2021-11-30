using Blazored.Modal;
using Blazored.Modal.Services;
//using Grpc.Net.Client;
using Microsoft.AspNetCore.Components;
using Notes2022.Shared;
using Syncfusion.Blazor.Grids;
using System.Net.Http.Json;

namespace Notes2022.RCL.User.Dialogs
{
    /// <summary>
    /// Access editor for a files access tokens
    /// </summary>
    public partial class AccessList
    {
        [CascadingParameter] public IModalService Modal { get; set; }
        [CascadingParameter] public BlazoredModalInstance ModalInstance { get; set; }
        
        /// <summary>
        /// File Id we are working on
        /// </summary>
        [Parameter] public int fileId { get; set; }

        /// <summary>
        /// Grid of tokens
        /// </summary>
        private SfGrid<NoteAccess> MyGrid;

        /// <summary>
        /// List of tokens
        /// </summary>
        private List<NoteAccess> myList { get; set; }

        /// <summary>
        /// Temp list of tokens
        /// </summary>
        private List<NoteAccess> temp { get; set; }

        /// <summary>
        /// List of all users
        /// </summary>
        private List<UserData> userList { get; set; }

        /// <summary>
        /// My access
        /// </summary>
        private NoteAccess myAccess { get; set; }
        private int arcId { get; set; }

        /// <summary>
        /// message to display
        /// </summary>
        private string message { get; set; }

        [Inject] HttpClient Http { get; set; }
        [Inject] Blazored.SessionStorage.ISessionStorageService sessionStorage { get; set; }
        public AccessList()
        {
        }

        protected async override Task OnParametersSetAsync()
        {
            arcId = await sessionStorage.GetItemAsync<int>("ArcId");

            // get access tokens
            temp = await DAL.GetAccessList(Http, fileId);
            myList = new List<NoteAccess>();

            // get only those for our archive
            foreach (NoteAccess item in temp)
            {
                if (item.ArchiveId == arcId)
                {
                    myList.Add(item);
                }
            }

            // get the full list of users
            userList = await DAL.GetUserList(Http);

            // get my acccess token
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

        /// <summary>
        /// We are done
        /// </summary>
        private void Cancel()
        {
            ModalInstance.CancelAsync();
        }

        /// <summary>
        /// Add a new token for another user
        /// </summary>
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

        /// <summary>
        /// Item deleted - refresh list
        /// </summary>
        /// <param name="newMessage"></param>
        /// <returns></returns>
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
