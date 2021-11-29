using System.Net.Http.Json;
using System.Web;

namespace Notes2022.Shared
{

    /// <summary>
    /// 
    /// This is a Data Access Layer for Notes2022.  All UI interaction with 
    /// the Data on the Server takes place through here.  Data is pulled from 
    /// and pushed to the server through these methods.
    /// 
    /// The first parameter of every method is an HttpClient which enables 
    /// the DAL to talk to the Server.  On the Client side, HttpClient
    /// is "Injected" into the pages that require it.  (Most pages do.)
    /// 
    /// These methods are functionally oriented and not intended to directly
    /// access entities (Tables/Rows) although they do effect them and
    /// return them as a part of functional requests.
    /// 
    /// These methods will silently fail to do the requested action in some
    /// cases.  For example:  If the user does not have read access to a
    /// file they may not get data back.  If they do not have write access
    /// an attempt to create a note will fail silently...  The User Interface
    /// should prevent such attempts, but the server may enforce...
    /// 
    /// This is a rather minimal set.  Other functional features could call
    /// for additional methods and matching controllers on the server.
    /// For example: The database has a table that can be used to "Mark"
    /// notes for later output.  But the methods are not yet here or on
    /// the server.  Still other functions might call for added columns or
    /// even tables.  This can be done although it's more involved.
    /// 
    /// For a summary view of the datatables involved see the file
    /// NoteFile.cd under the Entities folder in this project.  For a
    /// more detailed look see the classes representing those entities
    /// in the same folder. These classes directly represent the database.
    /// The database was created FROM these classes in a "code-first"
    /// approach.  See the Notes2022.Server project folder Data, class
    /// NotesDbContext.cs.  This defines the database tables and how they
    /// relate to one another.  (Note:  The Database has a few other tables
    /// added for user Identity, Roles, etc...  These begin with "AspNet".
    /// Another extra set of tables begin with "HangFire."  These are added
    /// by the background job processor, Hangfire.)  Final note: The
    /// "AspNetUsers" table has had extra columns added for user
    /// preferences.  See the Models folder in the Notes2022.Server
    /// project for the added columns.
    /// 
    /// Sometimes the communication with the server is through non-database
    /// classes.  These are found in the NotesMoels folder in this project.
    /// See the classes or the InternalModels.cd file for a summary.
    /// 
    /// </summary>
    public static class DAL
    {
        #region About

        /// <summary>
        /// Gets info used on the About page.  Prime Admin and server 
        /// start time.
        /// </summary>
        /// <param name="Http"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Gets the list of access tokens for the file.
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="fileId">Id of file</param>
        /// <returns></returns>
        public static async Task<List<NoteAccess>> GetAccessList(HttpClient Http, int fileId)
        {
            List<NoteAccess> model = await Http.GetFromJsonAsync<List<NoteAccess>>("api/accesslist/" + fileId);
            return model;
        }

        /// <summary>
        /// Create a new Access token
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="item">Token to create</param>
        /// <returns></returns>
        public static async Task CreateAccessItem(HttpClient Http, NoteAccess item)
        {
            await Http.PostAsJsonAsync("api/AccessList", item);
        }

        /// <summary>
        /// Updates an Acccess token
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="item">new value of the token</param>
        /// <returns></returns>
        public static async Task UpdateAccessItem(HttpClient Http, NoteAccess item)
        {
            await Http.PutAsJsonAsync("api/accesslist", item);
        }

        /// <summary>
        /// Deletes an Access token given its key
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="NoteFileId">File Id</param>
        /// <param name="ArchiveId">Archive Id</param>
        /// <param name="UserID">UserId</param>
        /// <returns></returns>
        public static async Task DeleteAccessItem(HttpClient Http, int NoteFileId, int ArchiveId, string UserID)
        {
            string encoded = "api/accesslist/" + NoteFileId + "." + ArchiveId + "." + UserID;
            await Http.DeleteAsync(encoded);

        }

        /// <summary>
        /// Gets access token for the current user in the given file
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="FileId"></param>
        /// <returns></returns>
        public static async Task<NoteAccess> GetMyAccess(HttpClient Http, int FileId)
        {
            return await Http.GetFromJsonAsync<NoteAccess>("api/myaccess/" + FileId);
        }

        #endregion
        #region HomePageModel
        /// <summary>
        /// Gets set of data needed for admin page
        /// </summary>
        /// <param name="Http"></param>
        /// <returns></returns>
        public static async Task<HomePageModel> GetAdminPageData(HttpClient Http)
        {
            HomePageModel model = await Http.GetFromJsonAsync<HomePageModel>("api/AdminPageData");
            return model;
        }

        /// <summary>
        /// Gets data relevent to current user for the Home Page
        /// </summary>
        /// <param name="Http"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Sends an Email
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="stuff"></param>
        /// <returns></returns>
        public static async Task SendEmail(HttpClient Http, EmailModel stuff)
        {
            await Http.PostAsJsonAsync("api/Email", stuff);
        }
        #endregion
        #region Export/Import
        /// <summary>
        /// Gets note content for a given note
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="id">Id of the note</param>
        /// <returns></returns>
        public static async Task<NoteContent> GetExport2(HttpClient Http, long id)
        {
            return await Http.GetFromJsonAsync<NoteContent>("api/Export2/" + id);
        }

        /// <summary>
        /// Gets list of headers satisfying the input parameters.
        /// one ... many
        /// The noteOrd and respOrd being 0 indicates to get them all
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="fileid">file id</param>
        /// <param name="arcid">archive id</param>
        /// <param name="noteOrd">note ordinal</param>
        /// <param name="respOrd">response ordinal</param>
        /// <returns></returns>
        public static async Task<List<NoteHeader>> GetExport(HttpClient Http, int fileid, int arcid, int noteOrd, int respOrd)
        {
            string req = "" + fileid + "." + arcid + "." + noteOrd + "." + respOrd;
            return await Http.GetFromJsonAsync<List<NoteHeader>>("api/Export/" + req);
        }

        /// <summary>
        /// Get representation of file for Json output
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="fileid"></param>
        /// <param name="arcid"></param>
        /// <returns></returns>
        public static async Task<JsonExport> GetExportJson(HttpClient Http, int fileid, int arcid)
        {
            string req = fileid.ToString() + "." + arcid.ToString();
            return await Http.GetFromJsonAsync<JsonExport>("api/ExportJson/" + req);
        }

        /// <summary>
        /// Forward a note/string to an email address
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="stuff"></param>
        /// <returns></returns>
        public static async Task Forward(HttpClient Http, ForwardViewModel stuff)
        {
            await Http.PostAsJsonAsync("api/Forward", stuff);
        }

        /// <summary>
        /// Import a text file on the server
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="NoteFile"></param>
        /// <param name="UploadFile"></param>
        /// <returns></returns>
        public static async Task<bool> Import(HttpClient Http, string NoteFile, string UploadFile)
        {
            return await Http.GetFromJsonAsync<bool>("api/Import/" + NoteFile + "/" + UploadFile);
        }

        #endregion
        #region Linked
        /// <summary>
        /// Test if remote uri is an available Notes2022 system
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static async Task<bool> LinkTest(HttpClient Http, string uri)
        {
            string appUriEncoded = HttpUtility.UrlEncode(uri);
            return await Http.GetFromJsonAsync<bool>("api/LinkTest/" + appUriEncoded);
        }

        /// <summary>
        /// Test if target note file exists on target system
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="uri"></param>
        /// <param name="remoteFile"></param>
        /// <returns></returns>
        public static async Task<bool> LinkTest2(HttpClient Http, string uri, string remoteFile)
        {
            string appUriEncoded = HttpUtility.UrlEncode(uri);
            return await Http.GetFromJsonAsync<bool>("api/LinkTest2/" + appUriEncoded + "/" + remoteFile);
        }

        /// <summary>
        /// Gets the list of linked files
        /// </summary>
        /// <param name="Http"></param>
        /// <returns></returns>
        public static async Task<List<LinkedFile>> GetLinked(HttpClient Http)
        {
            return await Http.GetFromJsonAsync<List<LinkedFile>>("api/Linked");
        }

        /// <summary>
        /// Create a link for a file
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="linked"></param>
        /// <returns></returns>
        public static async Task CreateLinked(HttpClient Http, LinkedFile linked)
        {
            await Http.PostAsJsonAsync<LinkedFile>("api/Linked", linked);
        }

        /// <summary>
        /// Update a linked file
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="linked"></param>
        /// <returns></returns>
        public static async Task UpdateLinked(HttpClient Http, LinkedFile linked)
        {
            await Http.PutAsJsonAsync<LinkedFile>("api/Linked", linked);
        }

        /// <summary>
        /// Delete a link for a file
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static async Task DeleteLinked(HttpClient Http, int Id)
        {
            await Http.DeleteAsync("api/Linked/" + Id);
        }

        #endregion
        #region Note

        /// <summary>
        /// Get the note header for the last created note.
        /// </summary>
        /// <param name="Http"></param>
        /// <returns></returns>
        public static async Task<NoteHeader> GetNewNote2(HttpClient Http)
        {
            return await Http.GetFromJsonAsync<NoteHeader>("api/NewNote2");
        }

        /// <summary>
        /// Get a file for creating a new note
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="fileId">Id of file to get</param>
        /// <returns></returns>
        public static async Task<NoteFile> GetNewNote(HttpClient Http, int fileId)
        {
            return await Http.GetFromJsonAsync<NoteFile>("api/NewNote/" + fileId);
        }

        /// <summary>
        /// Create a new note from the input specs.
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="Model"></param>
        /// <returns></returns>
        public static async Task CreateNewNote(HttpClient Http, TextViewModel Model)
        {
            await Http.PostAsJsonAsync("api/NewNote/", Model);
        }

        /// <summary>
        /// Update a note
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="Model"></param>
        /// <returns></returns>
        public static async Task UpdateNote(HttpClient Http, TextViewModel Model)
        {
            await Http.PutAsJsonAsync("api/NewNote/", Model);
        }

        /// <summary>
        /// Get the file Id  for the given note Id.
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="NoteId"></param>
        /// <returns></returns>
        public static async Task<int> GetFileIdForNoteId(HttpClient Http, long NoteId)
        {
            return await Http.GetFromJsonAsync<int>("api/GetFIleIdForNoteId/" + NoteId);
        }

        /// <summary>
        /// Get the meat of a note for display
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="NoteId">Id of note</param>
        /// <param name="Vers">Version number -  0 by default</param>
        /// <returns></returns>
        public static async Task<DisplayModel> GetNoteContent(HttpClient Http, long NoteId, int Vers = 0)
        {
            return await Http.GetFromJsonAsync<DisplayModel>("api/notecontent/" + NoteId + "/" + Vers);
        }

        /// <summary>
        /// Get all the tags for a notefile
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="nfid"></param>
        /// <returns></returns>
        public static async Task<List<Tags>> GetTags(HttpClient Http, int nfid)
        {
            return await Http.GetFromJsonAsync<List<Tags>>("api/Tags/" + nfid);
        }

        /// <summary>
        /// Get the Versions of a note (other than V=0)
        /// Notes are Identified by file, ordinal, response ordinal, archive
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="fileid"></param>
        /// <param name="ordinal"></param>
        /// <param name="respordinal"></param>
        /// <param name="arcid"></param>
        /// <returns></returns>
        public static async Task<List<NoteHeader>> GetVersions(HttpClient Http, int fileid, int ordinal, int respordinal, int arcid)
        {
            return await Http.GetFromJsonAsync<List<NoteHeader>>("api/Versions/" + fileid + "/"
                + ordinal + "/" + respordinal + "/" + arcid);
        }

        /// <summary>
        /// Get info needed to display the note index
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="NotesfileId"></param>
        /// <returns></returns>
        public static async Task<NoteDisplayIndexModel> GetNoteIndex(HttpClient Http, int NotesfileId)
        {
            return await Http.GetFromJsonAsync<NoteDisplayIndexModel>("api/NoteIndex/" + NotesfileId);
        }

        /// <summary>
        /// Copy a note/string to another file
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="cm"></param>
        /// <returns></returns>
        public static async Task CopyNote(HttpClient Http, CopyModel cm)
        {
            await Http.PostAsJsonAsync("api/CopyNote", cm);
        }

        /// <summary>
        /// Delete a note (mark it as deleted)
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static async Task DeleteNote(HttpClient Http, long Id)
        {
            await Http.DeleteAsync("api/DeleteNote/" + Id);
        }
        #endregion
        #region NoteFileAdmin
        /// <summary>
        /// Get list of notefiles in alpha order by name
        /// </summary>
        /// <param name="Http"></param>
        /// <returns></returns>
        public static async Task<List<NoteFile>> GetNoteFilesOrderedByName(HttpClient Http)
        {
            return await Http.GetFromJsonAsync<List<NoteFile>>("api/NoteFileAdmin");
        }

        /// <summary>
        /// Create a notefile
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="Model"></param>
        /// <returns></returns>
        public static async Task CreateNoteFile(HttpClient Http, CreateFileModel Model)
        {
            await Http.PostAsJsonAsync("api/NoteFileAdmin", Model);
        }

        /// <summary>
        /// Update notefile information (name/title)
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="Model"></param>
        /// <returns></returns>
        public static async Task UpdateNoteFile(HttpClient Http, NoteFile Model)
        {
            await Http.PutAsJsonAsync("api/NoteFileAdmin", Model);
        }

        /// <summary>
        /// Delete a notefile
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="FileId"></param>
        /// <returns></returns>
        public static async Task DeleteNoteFile(HttpClient Http, int FileId)
        {
            await Http.DeleteAsync("api/NoteFileAdmin/" + FileId);
        }

        /// <summary>
        /// Create on of the "Standard" files
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static async Task CreateStdNoteFile(HttpClient Http, string filename)
        {
            await Http.PostAsJsonAsync("api/NoteFileAdminStd", new Stringy { value = filename });
        }
        #endregion
        #region Sequencer
        /// <summary>
        /// Get list of sequencers for a user
        /// </summary>
        /// <param name="Http"></param>
        /// <returns></returns>
        public static async Task<List<Sequencer>> GetSequencer(HttpClient Http)
        {
            return await Http.GetFromJsonAsync<List<Sequencer>>("api/sequencer");
        }

        /// <summary>
        /// Create a sequencer item
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="Model"></param>
        /// <returns></returns>
        public static async Task CreateSequencer(HttpClient Http, SCheckModel Model)
        {
            await Http.PostAsJsonAsync("api/sequencer", Model);
        }

        /// <summary>
        /// Delete a sequencer item
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="SequencerFileId"></param>
        /// <returns></returns>
        public static async Task DeleteSequencer(HttpClient Http, int SequencerFileId)
        {
            await Http.DeleteAsync("api/sequencer/" + SequencerFileId);
        }

        /// <summary>
        /// Update a sequencer item while reading recent notes
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="seq"></param>
        /// <returns></returns>
        public static async Task UpateSequencer(HttpClient Http, Sequencer seq)
        {
            await Http.PutAsJsonAsync("api/sequencer", seq);
        }

        /// <summary>
        /// Change the order of a sequencer item
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="seq"></param>
        /// <returns></returns>
        public static async Task UpateSequencerPosition(HttpClient Http, Sequencer seq)
        {
            await Http.PutAsJsonAsync("api/sequenceredit", seq);
        }
        #endregion
        #region Subscription
        /// <summary>
        /// Get the list of subscriptions for a user
        /// </summary>
        /// <param name="Http"></param>
        /// <returns></returns>
        public static async Task<List<Subscription>> GetSubscriptions(HttpClient Http)
        {
            return await Http.GetFromJsonAsync<List<Subscription>>("api/subscription");
        }

        /// <summary>
        /// Delete a subscription
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public static async Task DeleteSubscription(HttpClient Http, int fileId)
        {
            await Http.DeleteAsync("api/Subscription/" + fileId);
        }

        /// <summary>
        /// Create a subscription
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="Model"></param>
        /// <returns></returns>
        public static async Task CreateSubscription(HttpClient Http, SCheckModel Model)
        {
            await Http.PostAsJsonAsync("api/Subscription", Model);
        }

        #endregion
        #region UserData
        /// <summary>
        /// Get the user specific data for the current user
        /// </summary>
        /// <param name="Http"></param>
        /// <returns></returns>
        public static async Task<UserData> GetUserData(HttpClient Http)
        {
            return await Http.GetFromJsonAsync<UserData>("api/User");
        }

        /// <summary>
        /// Update a users data
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="userData"></param>
        /// <returns></returns>
        public static async Task UpdateUserData(HttpClient Http, UserData userData)
        {
            await Http.PutAsJsonAsync("api/User", userData);
        }

        /// <summary>
        /// Get user data for editing
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static async Task<EditUserViewModel> GetUserEdit(HttpClient Http, string UserId)
        {
            return await Http.GetFromJsonAsync<EditUserViewModel>("api/useredit/" + UserId);
        }

        /// <summary>
        /// Update a user
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task UpdateUser(HttpClient Http, EditUserViewModel model)
        {
            await Http.PutAsJsonAsync("api/useredit", model);
        }

        /// <summary>
        /// Gets the list of all users
        /// </summary>
        /// <param name="Http"></param>
        /// <returns></returns>
        public static async Task<List<UserData>> GetUserList(HttpClient Http)
        {
            return await Http.GetFromJsonAsync<List<UserData>>("api/userlists");
        }
        #endregion
    }
}
