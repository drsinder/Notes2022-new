using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notes2022.Server.Data;
using Notes2022.Server.Models;
using Notes2022.Shared;
using System.Security.Claims;
using System.Text;

namespace Notes2022.Server.Controllers
{
    /// <summary>
    /// Copy a note to another file
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CopyNoteController : ControllerBase
    {
        private NoteFile noteFile { get; set; }
        private UserData UserData { get; set; }

        private readonly NotesDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CopyNoteController(NotesDbContext db,
                UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor
                )
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Copy a note to another file
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task Post(CopyModel Model)
        {
            // Who am I?
            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            UserData = NoteDataManager.GetUserData(user);   // get my data

            int fileId = Model.FileId;

            // Can I write to the target file?
            string uid = UserData.UserId;
            NoteAccess myAccess = await AccessManager.GetAccess(_db, uid, fileId, 0);
            if (!myAccess.Write)
                return;         // can not write to file

            // Prepare to copy
            NoteHeader Header = Model.Note;
            bool whole = Model.WholeString;
            noteFile = await _db.NoteFile.SingleAsync(p => p.Id == fileId);

            // Just the note
            if (!whole)
            {
                NoteContent cont = await _db.NoteContent.SingleAsync(p => p.NoteHeaderId == Header.Id);
                //cont.NoteHeader = null;
                List<Tags> tags = await _db.Tags.Where(p => p.NoteHeaderId == Header.Id).ToListAsync();

                string Body = string.Empty;
                Body = MakeHeader(Header);
                Body += cont.NoteBody;

                Header = Header.CloneForLink();

                Header.Id = 0;
                Header.ArchiveId = 0;
                Header.LinkGuid = string.Empty;
                Header.NoteOrdinal = 0;
                Header.ResponseCount = 0;
                Header.NoteFileId = fileId;
                Header.BaseNoteId = 0;
                //Header.NoteFile = null;
                Header.AuthorID = UserData.UserId;
                Header.AuthorName = UserData.DisplayName;

                Header.CreateDate = Header.ThreadLastEdited = Header.LastEdited = DateTime.Now.ToUniversalTime();

                await NoteDataManager.CreateNote(_db, Header, Body, Tags.ListToString(tags), Header.DirectorMessage, true, false);

                return;
            }
            else    // whole note string
            {
                // get base note first
                NoteHeader BaseHeader;
                BaseHeader = await _db.NoteHeader.SingleAsync(p => p.NoteFileId == Header.NoteFileId
                    && p.ArchiveId == Header.ArchiveId
                    && p.NoteOrdinal == Header.NoteOrdinal
                    && p.ResponseOrdinal == 0);

                Header = BaseHeader.CloneForLink();

                NoteContent cont = await _db.NoteContent.SingleAsync(p => p.NoteHeaderId == Header.Id);
                //cont.NoteHeader = null;
                List<Tags> tags = await _db.Tags.Where(p => p.NoteHeaderId == Header.Id).ToListAsync();

                string Body = string.Empty;
                Body = MakeHeader(Header);
                Body += cont.NoteBody;

                Header.Id = 0;
                Header.ArchiveId = 0;
                Header.LinkGuid = string.Empty;
                Header.NoteOrdinal = 0;
                Header.ResponseCount = 0;
                Header.NoteFileId = fileId;
                Header.BaseNoteId = 0;
                //Header.NoteFile = null;
                Header.AuthorID = UserData.UserId;
                Header.AuthorName = UserData.DisplayName;

                Header.CreateDate = Header.ThreadLastEdited = Header.LastEdited = DateTime.Now.ToUniversalTime();

                Header.NoteContent = null;

                NoteHeader NewHeader = await NoteDataManager.CreateNote(_db, Header, Body, Tags.ListToString(tags), Header.DirectorMessage, true, false);

                // now deal with any responses
                for (int i = 1; i <= BaseHeader.ResponseCount; i++)
                {
                    NoteHeader RHeader = await _db.NoteHeader.SingleAsync(p => p.NoteFileId == BaseHeader.NoteFileId
                        && p.ArchiveId == BaseHeader.ArchiveId
                        && p.NoteOrdinal == BaseHeader.NoteOrdinal
                        && p.ResponseOrdinal == i);

                    Header = RHeader.CloneForLinkR();

                    cont = await _db.NoteContent.SingleAsync(p => p.NoteHeaderId == Header.Id);
                    tags = await _db.Tags.Where(p => p.NoteHeaderId == Header.Id).ToListAsync();

                    Body = string.Empty;
                    Body = MakeHeader(Header);
                    Body += cont.NoteBody;

                    Header.Id = 0;
                    Header.ArchiveId = 0;
                    Header.LinkGuid = string.Empty;
                    Header.NoteOrdinal = NewHeader.NoteOrdinal;
                    Header.ResponseCount = 0;
                    Header.NoteFileId = fileId;
                    Header.BaseNoteId = NewHeader.Id;
                    //Header.NoteFile = null;
                    Header.ResponseOrdinal = 0;
                    Header.AuthorID = UserData.UserId;
                    Header.AuthorName = UserData.DisplayName;

                    Header.CreateDate = Header.ThreadLastEdited = Header.LastEdited = DateTime.Now.ToUniversalTime();

                    await NoteDataManager.CreateResponse(_db, Header, Body, Tags.ListToString(tags), Header.DirectorMessage, true, false);
                }
            }
        }

        // Utility method - makes a viewable header for the copied note
        private string MakeHeader(NoteHeader header)
        {
            StringBuilder sb = new();

            sb.Append("<div class=\"copiednote\">From: ");
            sb.Append(noteFile.NoteFileName);
            sb.Append(" - ");
            sb.Append(header.NoteSubject);
            sb.Append(" - ");
            sb.Append(header.AuthorName);
            sb.Append(" - ");
            sb.Append(header.CreateDate.ToShortDateString());
            sb.AppendLine("</div>");
            return sb.ToString();
        }
    }
}