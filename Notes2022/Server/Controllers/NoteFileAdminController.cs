/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: NoteFileAdminController.cs
    **
    ** Description:
    **      Create, delete, edit, and get notes files (admin)
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
using Notes2022.Server.Data;
using Notes2022.Server.Models;
using Notes2022.Shared;
using System.Security.Claims;

namespace Notes2022.Server.Controllers
{
    [Route("api/[controller]")]
    [Route("api/[controller]/{id}")]
    [ApiController]
    public class NoteFileAdminController : ControllerBase
    {
        private readonly NotesDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NoteFileAdminController(NotesDbContext db,
                UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor
                )
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<List<NoteFile>> Get()
        {
            return await NoteDataManager.GetNoteFilesOrderedByName(_db);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task Post(CreateFileModel crm)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            bool test = await _userManager.IsInRoleAsync(user, "Admin");
            if (!test)
                return;

            await NoteDataManager.CreateNoteFile(_db, _userManager, userId, crm.NoteFileName, crm.NoteFileTitle);
        }

        //[Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task Delete(string id)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            bool test = await _userManager.IsInRoleAsync(user, "Admin");
            if (!test)
                return;

            int intid = int.Parse(id);
            await NoteDataManager.DeleteNoteFile(_db, intid);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task Put(NoteFile edited)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            bool test = await _userManager.IsInRoleAsync(user, "Admin");
            if (!test)
                return;

            NoteFile live = await _db.NoteFile.FindAsync(edited.Id);

            live.LastEdited = DateTime.Now.ToUniversalTime();
            live.NoteFileName = edited.NoteFileName;
            live.NoteFileTitle = edited.NoteFileTitle;
            live.OwnerId = edited.OwnerId;

            _db.Update(live);
            await _db.SaveChangesAsync();
        }
    }
}