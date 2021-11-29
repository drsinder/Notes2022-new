using Blazored.Modal;
using Blazored.Modal.Services;
//using Grpc.Net.Client;
using Microsoft.AspNetCore.Components;
using Notes2022.RCL.User.Dialogs;
using Notes2022.Shared;
using System.Net.Http.Json;
using System.Web;

namespace Notes2022.RCL.Admin.Dialogs
{
    public partial class CreateLinked
    {
        [CascadingParameter] BlazoredModalInstance ModalInstance { get; set; }
        private List<LinkedFile> Model { get; set; }
        private List<NoteFile> Files { get; set; }
        private int myFile { get; set; }
        private string remoteFile { get; set; }
        private string appUri { get; set; }
        private string secret { get; set; }
        private bool accept { get; set; }
        private bool send { get; set; }

        [Inject] HttpClient Http { get; set; }
        //[Inject] GrpcChannel Channel { get; set; }

        [Inject] IModalService Modal { get; set; }
        public CreateLinked()
        {
        }

        protected override async Task OnParametersSetAsync()
        {
            HomePageModel hpModel = await DAL.GetHomePageData(Http);
            Files = hpModel.NoteFiles;
            myFile = 0;
            accept = send = true;
            Model = await DAL.GetLinked(Http);
        }

        protected async Task Cancel()
        {
            await ModalInstance.CancelAsync();
        }

        protected async Task Submit()
        {
            if (myFile == 0)
            {
                await ShowMessage("Select a file!");
                return;
            }

            if (string.IsNullOrEmpty(remoteFile))
            {
                await ShowMessage("Enter a remote file!");
                return;
            }

            if (!appUri.EndsWith("/"))
            {
                appUri += "/";
            }

            bool result = await DAL.LinkTest(Http, appUri);

            if (!result)
            {
                await ShowMessage("Remote system not responding...");
                return;
            }

            result = await DAL.LinkTest2(Http, appUri, remoteFile);
            if (!result)
            {
                await ShowMessage("Remote file does not exist.");
                return;
            }

            LinkedFile linker = new LinkedFile
            {
                HomeFileId = myFile,
                HomeFileName = Files.Find(p => p.Id == myFile).NoteFileName,
                RemoteBaseUri = appUri,
                RemoteFileName = remoteFile,
                Secret = secret,
                SendTo = send,
                AcceptFrom = accept
            };

            await DAL.CreateLinked(Http, linker);

            await ModalInstance.CancelAsync();
        }

        private async Task ShowMessage(string message)
        {
            var parameters = new ModalParameters();
            parameters.Add("MessageInput", message);
            var xx = Modal.Show<MessageBox>("", parameters);
            await xx.Result;
        }
    }
}