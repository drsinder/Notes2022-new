/*--------------------------------------------------------------------------
    **
    ** Copyright© 2022, Dale Sinder
    **
    ** Name: MyAccessController.cs
    **
    ** Description:
    **      Get access item for login user
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
    **  If not, see<http: //www.gnu.org/licenses/gpl-3.0.txt>.
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
    [Route("api/[controller]/{Id:int}")]
    [Route("api/[controller]")]
    [ApiController]
    public class MyAccessController : ControllerBase
    {
        private readonly NotesDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MyAccessController(NotesDbContext db,
            UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor
            )
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Gets current user access to the given file
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<NoteAccess> Get(int Id)
        {
            // Who Am I?
            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser me = await _userManager.FindByIdAsync(userId);

            // get my access token
            NoteAccess mine = await _db.NoteAccess.Where(p => p.NoteFileId == Id && p.UserID == me.Id && p.ArchiveId == 0).OrderBy(p => p.ArchiveId).FirstOrDefaultAsync();

            if (mine is null)   // none?  Get Other token! Generic access token for file
            {
                mine = await _db.NoteAccess.Where(p => p.NoteFileId == Id && p.UserID == Globals.AccessOtherId() && p.ArchiveId == 0).OrderBy(p => p.ArchiveId).FirstOrDefaultAsync();
            }

            return mine;
        }

    }
}