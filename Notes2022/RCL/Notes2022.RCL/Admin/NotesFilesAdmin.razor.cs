using Blazored.Modal;
using Blazored.Modal.Services;
//using Grpc.Net.Client;
using Microsoft.AspNetCore.Components;
using Notes2022.RCL.Admin.Dialogs;
using Notes2022.Shared;
using System.Net.Http.Json;

namespace Notes2022.RCL.Admin
{
    public partial class NotesFilesAdmin
    {
        private List<string> todo { get; set; }
        private List<NoteFile> files { get; set; }
        private string? message;
        private HomePageModel model { get; set; }

        [Inject] HttpClient Http { get; set; }
        //[Inject] GrpcChannel Channel { get; set; }
        [Inject] NavigationManager Navigation { get; set; }
        [Inject] IModalService Modal { get; set; }
        public NotesFilesAdmin()
        {
        }

        protected override async Task OnParametersSetAsync()
        {
            await GetStuff();
        }

        protected async Task GetStuff()
        {
            model = await DAL.GetAdminPageData(Http);

            todo = new List<string> { "announce", "pbnotes", "noteshelp", "pad", "homepagemessages" };

            foreach (NoteFile file in model.NoteFiles)
            {
                if (file.NoteFileName == "announce")
                    todo.Remove("announce");
                if (file.NoteFileName == "pbnotes")
                    todo.Remove("pbnotes");
                if (file.NoteFileName == "noteshelp")
                    todo.Remove("noteshelp");
                if (file.NoteFileName == "pad")
                    todo.Remove("pad");
                if (file.NoteFileName == "homepagemessages")
                    todo.Remove("homepagemessages");
            }
            files = model.NoteFiles;
        }

        private async Task CreateAnnounce()
        {
            await DAL.CreateStdNoteFile(Http, "announce");
            Navigation.NavigateTo("admin/notefilelist", true);
        }

        private async Task CreatePbnotes()
        {
            await DAL.CreateStdNoteFile(Http, "pbnotes");
            Navigation.NavigateTo("admin/notefilelist", true);
        }

        private async Task CreateNotesHelp()
        {
            await DAL.CreateStdNoteFile(Http, "noteshelp");
            Navigation.NavigateTo("admin/notefilelist", true);
        }

        private async Task CreatePad()
        {
            await DAL.CreateStdNoteFile(Http, "pad");
            Navigation.NavigateTo("admin/notefilelist", true);
        }

        private async Task CreateHomePageMessages()
        {
            await DAL.CreateStdNoteFile(Http, "homepagemessages");
            Navigation.NavigateTo("admin/notefilelist", true);
        }

        async void CreateNoteFile(int Id)
        {
            StateHasChanged();
            var parameters = new ModalParameters();
            parameters.Add("FileId", Id);
            var xModal = Modal.Show<CreateNoteFile>("Create Notefile", parameters);
            var result = await xModal.Result;
            if (!result.Cancelled)
                Navigation.NavigateTo("admin/notefilelist", true);
        }

        async void DeleteNoteFile(int Id)
        {
            NoteFile file = files.Find(p => p.Id == Id);

            StateHasChanged();
            var parameters = new ModalParameters();
            parameters.Add("FileId", Id);
            parameters.Add("FileName", file.NoteFileName);
            parameters.Add("FileTitle", file.NoteFileTitle);
            var xModal = Modal.Show<DeleteNoteFile>("Delete", parameters);
            var result = await xModal.Result;
            if (!result.Cancelled)
                Navigation.NavigateTo("admin/notefilelist", true);
        }

        async void NoteFileDetails(int Id)
        {
            NoteFile file = files.Find(p => p.Id == Id);

            var parameters = new ModalParameters();
            parameters.Add("FileId", Id);
            parameters.Add("FileName", file.NoteFileName);
            parameters.Add("FileTitle", file.NoteFileTitle);
            parameters.Add("LastEdited", file.LastEdited);
            parameters.Add("NumberArchives", file.NumberArchives);
            parameters.Add("Owner", model.UserListData.Find(p => p.UserId == file.OwnerId).DisplayName);
            var xModal = Modal.Show<NoteFileDetails>("Details", parameters);
            await xModal.Result;
        }

        async void EditNoteFile(int Id)
        {

            NoteFile file = files.Find(p => p.Id == Id);

            var parameters = new ModalParameters();
            parameters.Add("FileId", Id);
            parameters.Add("FileName", file.NoteFileName);
            parameters.Add("FileTitle", file.NoteFileTitle);
            parameters.Add("LastEdited", file.LastEdited);
            parameters.Add("NumberArchives", file.NumberArchives);
            parameters.Add("Owner", file.OwnerId);
            var xModal = Modal.Show<EditNoteFile>("Edit Notefile", parameters);
            var result = await xModal.Result;
            if (!result.Cancelled)
                Navigation.NavigateTo("admin/notefilelist", true);
        }

        async Task ImportNoteFile(int Id)
        {
            var parameters = new ModalParameters();
            var xModal = Modal.Show<Upload1>("Upload1", parameters);
            var result = await xModal.Result;
            if (result.Cancelled)
            {
                return;
            }
            else
            {
                NoteFile file = files.Find(p => p.Id == Id);

                string filename = (string)result.Data;
                parameters = new ModalParameters();
                parameters.Add("UploadFile", filename);
                parameters.Add("NoteFile", file.NoteFileName);

                var yModal = Modal.Show<Upload2>("Upload2", parameters);
                await yModal.Result;

                Navigation.NavigateTo("noteindex/" + Id);
                return;
            }
        }

    }
}
