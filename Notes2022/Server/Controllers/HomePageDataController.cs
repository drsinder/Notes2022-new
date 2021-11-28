/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: HomePageDataController.cs
    **
    ** Description:
    **      Data items for Homepage - also used a couple other places.
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
    //[AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class HomePageDataController : ControllerBase
    {
        private readonly NotesDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomePageDataController(NotesDbContext db,
            UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor
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

            model.Message = string.Empty;
            NoteFile hpmf = _db.NoteFile.Where(p => p.NoteFileName == "homepagemessages").FirstOrDefault();
            if (hpmf is not null)
            {
                NoteHeader hpmh = _db.NoteHeader.Where(p => p.NoteFileId == hpmf.Id && p.IsDeleted == false).OrderByDescending(p => p.CreateDate).FirstOrDefault();
                if (hpmh is not null)
                {
                    model.Message = _db.NoteContent.Where(p => p.NoteHeaderId == hpmh.Id).FirstOrDefault().NoteBody;
                }
            }

            model.NoteFiles = _db.NoteFile
                .OrderBy(p => p.NoteFileName).ToList();

            model.NoteAccesses = new List<NoteAccess>();

            try
            {
                string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    ApplicationUser user = await _userManager.FindByIdAsync(userId);
                    model.UserData = NoteDataManager.GetUserData(user);

                    foreach (NoteFile nf in model.NoteFiles)
                    {
                        NoteAccess na = await AccessManager.GetAccess(_db, user.Id, nf.Id, 0);
                        model.NoteAccesses.Add(na);
                    }

                    if (model.NoteAccesses.Count > 0)
                    {
                        NoteFile[] theList = new NoteFile[model.NoteFiles.Count];
                        model.NoteFiles.CopyTo(theList);
                        foreach (NoteFile nf2 in theList)
                        {
                            NoteAccess na = model.NoteAccesses.SingleOrDefault(p => p.NoteFileId == nf2.Id);
                            if (!na.ReadAccess && !na.Write && !na.EditAccess)
                            {
                                model.NoteFiles.Remove(nf2);
                            }
                        }
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

            //Globals.GuestId = model.GuestId = "*none*";
            //UserData Gdata = _db.UserData.Where(p => p.DisplayName == "Guest").FirstOrDefault();
            //if (Gdata is not null)
            //{
            //    Globals.GuestId = model.GuestId = Gdata.UserId;

            //    IdentityUser me = await _userManager.FindByIdAsync(Globals.GuestId);

            //    model.GuestEmail = Globals.GuestEmail = me.Email;
            //}

            return model;
        }
    }
}
