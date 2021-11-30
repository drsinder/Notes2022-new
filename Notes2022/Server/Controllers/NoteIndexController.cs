/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: NoteIndexController.cs
    **
    ** Description:
    **      Supply data NOte file index (Main)
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
    //[Route("api/[controller]")]
    [Route("api/[controller]/{id:int}")]
    [ApiController]
    public class NoteIndexController : ControllerBase
    {
        private readonly NotesDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NoteIndexController(NotesDbContext db,
                UserManager<ApplicationUser> userManager,
                IHttpContextAccessor httpContextAccessor
                )
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Get Info to enable notes index display
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<NoteDisplayIndexModel> Get(int id)
        {
            NoteDisplayIndexModel idxModel = new NoteDisplayIndexModel();

            int arcId = 0;

            // Who am I?
            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            bool isUser = await _userManager.IsInRoleAsync(user, "User");
            if (!isUser)
                return idxModel;    // not a User?  You get NOTHING!

            idxModel.myAccess = await GetMyAccess(user, id, arcId);
            if (isAdmin)
            {
                idxModel.myAccess.ViewAccess = true; // Admins can always view access list
            }
            idxModel.noteFile = await NoteDataManager.GetFileById(_db, id);

            if (!idxModel.myAccess.ReadAccess && !idxModel.myAccess.Write)
            {
                idxModel.message = "You do not have access to file " + idxModel.noteFile.NoteFileName;
                return idxModel;
            }

            List<LinkedFile> linklist = await _db.LinkedFile.Where(p => p.HomeFileId == id).ToListAsync();
            if (linklist is not null && linklist.Count > 0)
                idxModel.linkedText = " (Linked)";

            // Add headers for file
            idxModel.AllNotes = await NoteDataManager.GetAllHeaders(_db, id, arcId);

            // Base note headers for file
            idxModel.Notes = idxModel.AllNotes.FindAll(p => p.ResponseOrdinal == 0).OrderBy(p => p.NoteOrdinal).ToList();

            idxModel.UserData = NoteDataManager.GetUserData(user);
            //idxModel.tZone = await LocalManager.GetUserTimeZone(user, _db);

            idxModel.Tags = await _db.Tags.Where(p => p.NoteFileId == id && p.ArchiveId == arcId).ToListAsync();

            idxModel.ArcId = arcId;

            return idxModel;
        }


        /// <summary>
        /// Get Access Control Object for file and user
        /// </summary>
        /// <param name="fileid"></param>
        /// <returns></returns>
        private async Task<NoteAccess> GetMyAccess(ApplicationUser me, int fileid, int ArcId)
        {

            NoteAccess noteAccess = await AccessManager.GetAccess(_db, me.Id, fileid, ArcId);

            if (User.IsInRole("Guest"))
                noteAccess.EditAccess = noteAccess.DeleteEdit = noteAccess.Respond = noteAccess.Write = false;

            return noteAccess;
        }

    }

}