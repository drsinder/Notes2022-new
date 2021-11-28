/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: AdminPageDataController.cs
    **
    ** Description:
    **      
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
    //[Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminPageDataController : Controller
    {
        private readonly NotesDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AdminPageDataController(NotesDbContext db,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }


        [HttpGet]
        public async Task<HomePageModel> Get()
        {
            HomePageModel model = new HomePageModel();

            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            bool test = await _userManager.IsInRoleAsync(user, "Admin");
            if (!test)
                return model;

            NoteFile hpmf = _db.NoteFile.Where(p => p.NoteFileName == "homepagemessages").FirstOrDefault();
            if (hpmf is not null)
            {
                NoteHeader hpmh = _db.NoteHeader.Where(p => p.NoteFileId == hpmf.Id).OrderByDescending(p => p.CreateDate).FirstOrDefault();
                if (hpmh is not null)
                {
                    model.Message = _db.NoteContent.Where(p => p.NoteHeaderId == hpmh.Id).FirstOrDefault().NoteBody;
                }
            }

            model.NoteFiles = _db.NoteFile
                .OrderBy(p => p.NoteFileName).ToList();

            model.NoteAccesses = new List<NoteAccess>();

            List<ApplicationUser> udl = _db.Users.ToList();

            model.UserListData = new List<UserData>();
            foreach (ApplicationUser userx in udl)
            {
                UserData ud = NoteDataManager.GetUserData(userx);
                model.UserListData.Add(ud);
            }

            try
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    model.UserData = NoteDataManager.GetUserData(user);

                    foreach (NoteFile nf in model.NoteFiles)
                    {
                        NoteAccess na = await AccessManager.GetAccess(_db, user.Id, nf.Id, 0);
                        model.NoteAccesses.Add(na);
                    }
                }
                else
                {
                    model.UserData = new UserData { TimeZoneID = Globals.TimeZoneDefaultID };
                }
            }
            catch
            {
                model.UserData = new UserData { TimeZoneID = Globals.TimeZoneDefaultID };
            }

            return model;
        }
    }
}
