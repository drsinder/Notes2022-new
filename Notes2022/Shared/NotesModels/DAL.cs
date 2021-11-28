using System.Net.Http.Json;
using System.Web;

namespace Notes2022.Shared
{
    public static class DAL
    {
        #region About
        public static async Task<AboutModel> GetAbout(HttpClient Http)
        {
            try
            {
                AboutModel model = await Http.GetFromJsonAsync<AboutModel>("api/About");
                return model;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return new AboutModel();
        }

        //public static async Task<AboutModel> GetAbout(GrpcChannel Channel)
        //{
        //    try
        //    {
        //        var client = Channel.CreateGrpcService<INotes2022Service>();
        //        return await client.GetAbout();
        //    }
        //    catch (Exception ex)
        //    {
        //        var x = ex.Message;
        //    }
        //    return null;
        //}
        #endregion
        #region AccessList

        public static async Task<List<NoteAccess>> GetAccessList(HttpClient Http, int fileId)
        {
            List<NoteAccess> model = await Http.GetFromJsonAsync<List<NoteAccess>>("api/accesslist/" + fileId);
            return model;
        }

        public static async Task CreateAccessItem(HttpClient Http, NoteAccess item)
        {
            await Http.PostAsJsonAsync("api/AccessList", item);
        }

        public static async Task UpdateAccessItem(HttpClient Http, NoteAccess item)
        {
            await Http.PutAsJsonAsync("api/accesslist", item);
        }

        public static async Task DeleteAccessItem(HttpClient Http, int NoteFileId, int ArchiveId, string UserID)
        {
            string encoded = "api/accesslist/" + NoteFileId + "." + ArchiveId + "." + UserID;
            await Http.DeleteAsync(encoded);

        }

        public static async Task<NoteAccess> GetMyAccess(HttpClient Http, int FileId)
        {
            return await Http.GetFromJsonAsync<NoteAccess>("api/myaccess/" + FileId);
        }

        #endregion
        #region HomePageModel

        public static async Task<HomePageModel> GetAdminPageData(HttpClient Http)
        {
            HomePageModel model = await Http.GetFromJsonAsync<HomePageModel>("api/AdminPageData");
            return model;
        }

        public static async Task<HomePageModel> GetHomePageData(HttpClient Http)
        {
            HomePageModel model = await Http.GetFromJsonAsync<HomePageModel>("api/HomePageData");
            return model;
        }
        
        //public static async Task<HomePageModel> GetAdminPageData(GrpcChannel Channel)
        //{
        //    try
        //    {
        //        var client = Channel.CreateGrpcService<INotes2022Service>();
        //        return await client.GetAdminPageData();
        //    }
        //    catch (Exception ex)
        //    {
        //        var x = ex.Message;
        //    }
        //    return null;
        //}

        //public static async Task<HomePageModel> GetHomePageData(GrpcChannel Channel)
        //{
        //    try
        //    {
        //        var client = Channel.CreateGrpcService<INotes2022Service>();
        //        return await client.GetHomePageData();
        //    }
        //    catch (Exception ex)
        //    {
        //        var x = ex.Message;
        //    }
        //    return null;
        //}

        #endregion
        #region Email
        public static async Task SendEmail(HttpClient Http, EmailModel stuff)
        {
            await Http.PostAsJsonAsync("api/Email", stuff);
        }
        #endregion
        #region Export
        public static async Task<NoteContent> GetExport2(HttpClient Http, long id)
        {
            return await Http.GetFromJsonAsync<NoteContent>("api/Export2/" + id);
        }

        public static async Task<List<NoteHeader>> GetExport(HttpClient Http, int fileid, int arcid, int noteOrd, int respOrd)
        {
            string req = "" + fileid + "." + arcid + "." + noteOrd + "." + respOrd;
            return await Http.GetFromJsonAsync<List<NoteHeader>>("api/Export/" + req);
        }

        public static async Task<JsonExport> GetExportJson(HttpClient Http, int fileid, int arcid)
        {
            string req = fileid.ToString() + "." + arcid.ToString();
            return await Http.GetFromJsonAsync<JsonExport>("api/ExportJson/" + req);
        }

        public static async Task Forward(HttpClient Http, ForwardViewModel stuff)
        {
            await Http.PostAsJsonAsync("api/Forward", stuff);
        }

        public static async Task<bool> Import(HttpClient Http, string NoteFile, string UploadFile)
        {
            return await Http.GetFromJsonAsync<bool>("api/Import/" + NoteFile + "/" + UploadFile);
        }

        #endregion
        #region Linked
        public static async Task<bool> LinkTest(HttpClient Http, string uri)
        {
            string appUriEncoded = HttpUtility.UrlEncode(uri);
            return await Http.GetFromJsonAsync<bool>("api/LinkTest/" + appUriEncoded);
        }

        public static async Task<bool> LinkTest2(HttpClient Http, string uri, string remoteFile)
        {
            string appUriEncoded = HttpUtility.UrlEncode(uri);
            return await Http.GetFromJsonAsync<bool>("api/LinkTest2/" + appUriEncoded + "/" + remoteFile);
        }

        public static async Task<List<LinkedFile>> GetLinked(HttpClient Http)
        {
            return await Http.GetFromJsonAsync<List<LinkedFile>>("api/Linked");
        }

        public static async Task CreateLinked(HttpClient Http, LinkedFile linked)
        {
            await Http.PostAsJsonAsync<LinkedFile>("api/Linked", linked);
        }

        public static async Task UpdateLinked(HttpClient Http, LinkedFile linked)
        {
            await Http.PutAsJsonAsync<LinkedFile>("api/Linked", linked);
        }

        public static async Task DeleteLinked(HttpClient Http, int Id)
        {
            await Http.DeleteAsync("api/Linked/" + Id);
        }

        #endregion
        #region Note
        public static async Task<NoteHeader> GetNewNote2(HttpClient Http)
        {
            return await Http.GetFromJsonAsync<NoteHeader>("api/NewNote2");
        }

        public static async Task<NoteFile> GetNewNote(HttpClient Http, int fileId)
        {
            return await Http.GetFromJsonAsync<NoteFile>("api/NewNote/" + fileId);
        }

        public static async Task CreateNewNote(HttpClient Http, TextViewModel Model)
        {
            await Http.PostAsJsonAsync("api/NewNote/", Model);
        }

        public static async Task UpdateNote(HttpClient Http, TextViewModel Model)
        {
            await Http.PutAsJsonAsync("api/NewNote/", Model);
        }

        public static async Task<int> GetFileIdForNoteId(HttpClient Http, long NoteId)
        {
            return await Http.GetFromJsonAsync<int>("api/GetFIleIdForNoteId/" + NoteId);
        }

        public static async Task<DisplayModel> GetNoteContent(HttpClient Http, long NoteId, int Vers = 0)
        {
            return await Http.GetFromJsonAsync<DisplayModel>("api/notecontent/" + NoteId + "/" + Vers);
        }

        public static async Task<List<Tags>> GetTags(HttpClient Http, int nfid)
        {
            return await Http.GetFromJsonAsync<List<Tags>>("api/Tags/" + nfid);
        }

        public static async Task<List<NoteHeader>> GetVersions(HttpClient Http, int fileid, int ordinal, int respordinal, int arcid)
        {
            return await Http.GetFromJsonAsync<List<NoteHeader>>("api/Versions/" + fileid + "/"
                + ordinal + "/" + respordinal + "/" + arcid);
        }

        public static async Task<NoteDisplayIndexModel> GetNoteIndex(HttpClient Http, int NotesfileId)
        {
            return await Http.GetFromJsonAsync<NoteDisplayIndexModel>("api/NoteIndex/" + NotesfileId);
        }

        public static async Task CopyNote(HttpClient Http, CopyModel cm)
        {
            await Http.PostAsJsonAsync("api/CopyNote", cm);
        }

        public static async Task DeleteNote(HttpClient Http, long Id)
        {
            await Http.DeleteAsync("api/DeleteNote/" + Id);
        }
        #endregion
        #region NoteFileAdmin

        public static async Task<List<NoteFile>> GetNoteFilesOrderedByName(HttpClient Http)
        {
            return await Http.GetFromJsonAsync<List<NoteFile>>("api/NoteFileAdmin");
        }

        public static async Task CreateNoteFile(HttpClient Http, CreateFileModel Model)
        {
            await Http.PostAsJsonAsync("api/NoteFileAdmin", Model);
        }

        public static async Task UpdateNoteFile(HttpClient Http, NoteFile Model)
        {
            await Http.PutAsJsonAsync("api/NoteFileAdmin", Model);
        }

        public static async Task DeleteNoteFile(HttpClient Http, int FileId)
        {
            await Http.DeleteAsync("api/NoteFileAdmin/" + FileId);
        }

        public static async Task CreateStdNoteFile(HttpClient Http, string filename)
        {
            await Http.PostAsJsonAsync("api/NoteFileAdminStd", new Stringy { value = filename });
        }
        #endregion
        #region Sequencer

        public static async Task<List<Sequencer>> GetSequencer(HttpClient Http)
        {
            return await Http.GetFromJsonAsync<List<Sequencer>>("api/sequencer");
        }

        public static async Task CreateSequencer(HttpClient Http, SCheckModel Model)
        {
            await Http.PostAsJsonAsync("api/sequencer", Model);
        }

        public static async Task DeleteSequencer(HttpClient Http, int SequencerFileId)
        {
            await Http.DeleteAsync("api/sequencer/" + SequencerFileId);
        }

        public static async Task UpateSequencer(HttpClient Http, Sequencer seq)
        {
            await Http.PutAsJsonAsync("api/sequencer", seq);
        }

        public static async Task UpateSequencerPosition(HttpClient Http, Sequencer seq)
        {
            await Http.PutAsJsonAsync("api/sequenceredit", seq);
        }
        #endregion
        #region Subscription
        public static async Task<List<Subscription>> GetSubscriptions(HttpClient Http)
        {
            return await Http.GetFromJsonAsync<List<Subscription>>("api/subscription");
        }

        public static async Task DeleteSubscription(HttpClient Http, int fileId)
        {
            await Http.DeleteAsync("api/Subscription/" + fileId);
        }

        public static async Task CreateSubscription(HttpClient Http, SCheckModel Model)
        {
            await Http.PostAsJsonAsync("api/Subscription", Model);
        }

        #endregion
        #region UserData
        public static async Task<UserData> GetUserData(HttpClient Http)
        {
            return await Http.GetFromJsonAsync<UserData>("api/User");
        }

        public static async Task UpdateUserData(HttpClient Http, UserData userData)
        {
            await Http.PutAsJsonAsync("api/User", userData);
        }

        public static async Task<EditUserViewModel> GetUserEdit(HttpClient Http, string UserId)
        {
            return await Http.GetFromJsonAsync<EditUserViewModel>("api/useredit/" + UserId);
        }

        public static async Task UpdateUser(HttpClient Http, EditUserViewModel model)
        {
            await Http.PutAsJsonAsync("api/useredit", model);
        }

        public static async Task<List<UserData>> GetUserList(HttpClient Http)
        {
            return await Http.GetFromJsonAsync<List<UserData>>("api/userlists");
        }
        #endregion
    }
}
