/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: NoteContentController.cs
    **
    ** Description:
    **      Gets note content and tags
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
    [Route("api/[controller]")]
    [Route("api/[controller]/{id:long}/{vers:int}")]
    [ApiController]
    public class NoteContentController : ControllerBase
    {
        private readonly NotesDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NoteContentController(NotesDbContext db,
            UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<DisplayModel> Get(long id, int vers)
        {

            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            NoteHeader nh = await _db.NoteHeader.SingleAsync(p => p.Id == id && p.Version == vers);
            NoteContent c = await _db.NoteContent.SingleAsync(p => p.NoteHeaderId == nh.Id);
            List<Tags> tags = await _db.Tags.Where(p => p.NoteHeaderId == nh.Id).ToListAsync();
            NoteFile nf = await _db.NoteFile.SingleAsync(p => p.Id == nh.NoteFileId);

            NoteAccess access = await AccessManager.GetAccess(_db, userId, nh.NoteFileId, nh.ArchiveId);

            bool canEdit = await _userManager.IsInRoleAsync(user, "Admin");
            if (userId == nh.AuthorID)
                canEdit = true;

            return new DisplayModel { header = nh, content = c, tags = tags, noteFile = nf, access = access, CanEdit = canEdit, IsAdmin = isAdmin };
        }

    }
}