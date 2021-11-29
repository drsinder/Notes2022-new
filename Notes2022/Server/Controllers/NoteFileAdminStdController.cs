/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: NoteFileAdminStdController.cs
    **
    ** Description:
    **      creates the four "standard" notes files w appropriate access list
    **
    ** This program is free software: you can redistribute it and/or modify
    ** it under the terms of the GNU General Public License version 3 as
    ** published by the Free Software Foundation.   
    **
    ** This program is distributed in the hope that it will be useful,
    ** but WITHOUT ANY WARRANTY; without even the implied warranty of
    ** MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
    ** GNU General Public License version 3 for more details.
    **
    **  You should have received a copy of the GNU General Public License
    **  version 3 along with this program in file "license-gpl-3.0.txt".
    **  If not, see<http://www.gnu.org/licenses/gpl-3.0.txt>.
    **
    **--------------------------------------------------------------------------*/


using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notes2022.Server.Data;
using Notes2022.Server.Models;
using Notes2022.Shared;
using System.Security.Claims;

namespace Notes2022.Server.Controllers
{
    /// <summary>
    /// Creates one of the 5 standard file for an installation
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class NoteFileAdminStdController : Controller
    {
        private readonly NotesDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NoteFileAdminStdController(NotesDbContext db,
                UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor
                )
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Create a STD file by name
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task Post(Stringy file)
        {
            switch (file.value)
            {
                case "announce":
                    await CreateAnnounce();
                    break;

                case "pbnotes":
                    await CreatePbnotes();
                    break;

                case "noteshelp":
                    await CreateNoteshelp();
                    break;

                case "pad":
                    await CreatePad();
                    break;

                case "homepagemessages":
                    await CreateHomePageMessages();
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// The actual create is done here
        /// </summary>
        /// <param name="name"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public async Task<bool> CreateNoteFile(string name, string title)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            bool test = await _userManager.IsInRoleAsync(user, "Admin");
            if (!test)
                return false;

            return await NoteDataManager.CreateNoteFile(_db, _userManager, userId, name, title);
        }

        public async Task CreateHomePageMessages()
        {
            await CreateNoteFile("homepagemessages", "Home Page Messages");
            NoteFile nf4 = await NoteDataManager.GetFileByName(_db, "announce");
        }

        public async Task CreateAnnounce()
        {
            await CreateNoteFile("announce", "Notes 2022 Announcements");
            NoteFile nf4 = await NoteDataManager.GetFileByName(_db, "announce");

            // set read access for Other in Announce
            int padid = nf4.Id;
            NoteAccess access = await AccessManager.GetOneAccess(_db, Globals.AccessOtherId(), padid, 0);
            access.ReadAccess = true;

            _db.Entry(access).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task CreatePbnotes()
        {
            await CreateNoteFile("pbnotes", "Public Notes");
            NoteFile nf4 = await NoteDataManager.GetFileByName(_db, "pbnotes");

            // set read, write, respond access to Pbnotes
            int padid = nf4.Id;
            NoteAccess access = await AccessManager.GetOneAccess(_db, Globals.AccessOtherId(), padid, 0);
            access.ReadAccess = true;
            access.Respond = true;
            access.Write = true;

            _db.Entry(access).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task CreateNoteshelp()
        {
            await CreateNoteFile("noteshelp", "Help with Notes 2022");
            NoteFile nf4 = await NoteDataManager.GetFileByName(_db, "noteshelp");

            // Set read, write, respond access to Noteshelp
            int padid = nf4.Id;
            NoteAccess access = await AccessManager.GetOneAccess(_db, Globals.AccessOtherId(), padid, 0);
            access.ReadAccess = true;
            access.Respond = true;
            access.Write = true;

            _db.Entry(access).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task CreatePad()
        {
            await CreateNoteFile("pad", "Traditional Pad");
            NoteFile nf4 = await NoteDataManager.GetFileByName(_db, "pad");

            // Set read, write, respond access to pad
            int padid = nf4.Id;
            NoteAccess access = await AccessManager.GetOneAccess(_db, Globals.AccessOtherId(), padid, 0);
            access.ReadAccess = true;
            access.Respond = true;
            access.Write = true;

            _db.Entry(access).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }
    }
}
