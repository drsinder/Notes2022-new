/*--------------------------------------------------------------------------
    **
    ** Copyright© 2022, Dale Sinder
    **
    ** Name: ForwardController.cs
    **
    ** Description:
    **      Forward a note by email
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


using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notes2022.Server.Data;
using Notes2022.Server.Models;
using Notes2022.Server.Services;
using Notes2022.Shared;
using System.Security.Claims;

namespace Notes2022.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForwardController : ControllerBase
    {
        private readonly NotesDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ForwardController(NotesDbContext db,
                UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor
                )
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Forward to an email address
        /// </summary>
        /// <param name="fv"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task Post(ForwardViewModel fv)
        {
            // Who am I?
            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            UserData ud = NoteDataManager.GetUserData(user);

            // make the email
            string myEmail = await LocalService.MakeNoteForEmail(fv, fv.NoteFile, _db, ud.Email, ud.DisplayName);

            // Enqueue the mail for sending
            EmailSender emailSender = new();
            BackgroundJob.Enqueue(() => emailSender.SendEmailAsync(ud.Email, fv.NoteSubject, myEmail));
        }

    }

}
