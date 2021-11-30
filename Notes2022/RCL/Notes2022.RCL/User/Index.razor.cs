using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Notes2022.Shared;
using System.Net.Http.Json;
using System.Timers;

namespace Notes2022.RCL.User
{
    public partial class Index
    {
        /// <summary>
        /// Alternate name for NoteFile
        /// </summary>
        private class localFile : NoteFile
        {
        }

        [Parameter] public bool IsPreview { get; set; } = false;

        /// <summary>
        /// Place holder
        /// </summary>
        private localFile dummyFile = new localFile { Id = 0, NoteFileName = " ", NoteFileTitle = " " };

        /// <summary>
        /// List of files ordered by filename
        /// </summary>
        private List<localFile> fileList { get; set; }

        /// <summary>
        /// List of files ordered by title
        /// </summary>
        private List<localFile> nameList { get; set; }

        /// <summary>
        /// Important file list
        /// </summary>
        private List<localFile> impfileList { get; set; }

        /// <summary>
        /// History file list
        /// </summary>
        private List<localFile> histfileList { get; set; }

        /// <summary>
        /// Model for communications between client and server
        /// </summary>
        private HomePageModel? hpModel { get; set; }

        /// <summary>
        /// Current time
        /// </summary>
        private DateTime mytime { get; set; }

        // For clock update
        private System.Timers.Timer timer2 { get; set; }

        /// <summary>
        /// For access to server via Http
        /// </summary>
        [Inject] HttpClient Http { get; set; }
        [Inject] AuthenticationStateProvider AuthProv { get; set; }
        [Inject] NavigationManager Navigation { get; set; }
        [Inject] Blazored.SessionStorage.ISessionStorageService sessionStorage { get; set; }
        public Index()  // Needed for above Injection
        {
        }

        /// <summary>
        /// Called by framework when parameters haev been set.  Time to go get data.
        /// </summary>
        /// <returns></returns>
        protected override async Task OnParametersSetAsync()
        {
            fileList = new List<localFile>();
            nameList = new List<localFile>();
            histfileList = new List<localFile>();
            impfileList = new List<localFile>();

            mytime = DateTime.Now;

            if (IsPreview)
                return;

            // See if user is authenticated
            AuthenticationState authstate = await AuthProv.GetAuthenticationStateAsync();
            if (authstate.User.Identity.IsAuthenticated) // Yes, get and set data.
            {
                // Set and reset local state vars
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
                    hpModel = await DAL.GetHomePageData(Http);  // get our data from server

                    // Order files by name and title
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

                        // handle special important and history files
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

        /// <summary>
        /// Update the clock once per second
        /// </summary>
        /// <param name="firstRender"></param>
        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                timer2 = new System.Timers.Timer(1000);
                timer2.Elapsed += TimerTick2;
                timer2.Enabled = true;
            }
        }

        /// <summary>
        /// Handle typed in file name
        /// </summary>
        /// <param name="value"></param>
        protected void TextHasChanged(string value)
        {
            value = value.Trim().Replace("'\n", "").Replace("'\r", "").Replace(" ", "");

            try
            {
                foreach (var item in fileList)
                {
                    if (value == item.NoteFileName)
                    {
                        Navigation.NavigateTo("noteindex/" + item.Id); // goto the file
                        return;
                    }
                }
            }
            catch { }
        }

        protected void TimerTick2(Object source, ElapsedEventArgs e)
        {
            mytime = DateTime.Now;
            StateHasChanged();
        }
    }
}
