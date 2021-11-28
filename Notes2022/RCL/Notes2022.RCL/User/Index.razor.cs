//using Grpc.Net.Client;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Notes2022.Shared;
using System.Net.Http.Json;

namespace Notes2022.RCL.User
{
    public partial class Index
    {
        private class localFile : NoteFile
        {
        }

        [Parameter] public bool IsPreview { get; set; } = false;

        private localFile dummyFile = new localFile { Id = 0, NoteFileName = " ", NoteFileTitle = " " };
        private List<localFile> fileList { get; set; }
        private List<localFile> nameList { get; set; }
        private List<localFile> impfileList { get; set; }
        private List<localFile> histfileList { get; set; }

        private HomePageModel? hpModel { get; set; }
        private DateTime mytime { get; set; }


        [Inject] HttpClient Http { get; set; }
        //[Inject] GrpcChannel Channel { get; set; }
        [Inject] AuthenticationStateProvider AuthProv { get; set; }
        [Inject] NavigationManager Navigation { get; set; }
        [Inject] Blazored.SessionStorage.ISessionStorageService sessionStorage { get; set; }
        public Index()
        {
        }

        protected override async Task OnParametersSetAsync()
        {
            fileList = new List<localFile>();
            nameList = new List<localFile>();
            histfileList = new List<localFile>();
            impfileList = new List<localFile>();

            mytime = DateTime.Now;

            if (IsPreview)
                return;

            AuthenticationState authstate = await AuthProv.GetAuthenticationStateAsync();
            if (authstate.User.Identity.IsAuthenticated)
            {
                await sessionStorage.SetItemAsync<int>("ArcId", 0);
                await sessionStorage.SetItemAsync<int>("IndexPage", 1);

                await sessionStorage.SetItemAsync<bool>("IsSeq", false);
                await sessionStorage.RemoveItemAsync("SeqList");
                await sessionStorage.RemoveItemAsync("SeqItem");
                await sessionStorage.RemoveItemAsync("SeqIndex");

                await sessionStorage.RemoveItemAsync("SeqHeaders");
                await sessionStorage.RemoveItemAsync("SeqHeaderIndex");
                await sessionStorage.RemoveItemAsync("CurrentSeqHeader");

                await sessionStorage.SetItemAsync<bool>("InSearch", false);
                await sessionStorage.RemoveItemAsync("SearchIndex");
                await sessionStorage.RemoveItemAsync("SearchList");

                try
                {
                    hpModel = await DAL.GetHomePageData(Http);

                    List<NoteFile> fileList1 = hpModel.NoteFiles.OrderBy(p => p.NoteFileName).ToList();
                    List<NoteFile> nameList1 = hpModel.NoteFiles.OrderBy(p => p.NoteFileTitle).ToList();
                    histfileList = new List<localFile>();
                    impfileList = new List<localFile>();


                    for (int i = 0; i < fileList1.Count; i++)
                    {
                        localFile work = new localFile { Id = fileList1[i].Id, NoteFileName = fileList1[i].NoteFileName, NoteFileTitle = fileList1[i].NoteFileTitle };
                        localFile work2 = new localFile { Id = nameList1[i].Id, NoteFileName = nameList1[i].NoteFileName, NoteFileTitle = nameList1[i].NoteFileTitle };
                        fileList.Add(work);
                        nameList.Add(work2);

                        string fname = work.NoteFileName;
                        if (fname == "Opbnotes" || fname == "Gnotes")
                            histfileList.Add(work);

                        if (fname == "announce" || fname == "pbnotes" || fname == "noteshelp")
                            impfileList.Add(work);
                    }
                }
                finally { }
            }
        }

        protected void TextHasChanged(string value)
        {
            value = value.Trim().Replace("'\n", "").Replace("'\r", "").Replace(" ", "");

            try
            {
                foreach (var item in fileList)
                {
                    if (value == item.NoteFileName)
                    {
                        Navigation.NavigateTo("noteindex/" + item.Id);
                        return;
                    }
                }
            }
            catch { }
        }
    }
}
