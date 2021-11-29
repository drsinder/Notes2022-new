/*--------------------------------------------------------------------------
    **
    ** Copyright© 2022, Dale Sinder
    **
    ** Name: SubscriptionController.cs
    **
    ** Description:
    **      Manage subscriptions
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
using Notes2022.Server;
using Notes2022.Server.Data;
using Notes2022.Server.Models;
using Notes2022.Shared;
using System.Security.Claims;


namespace Notes2022.Server.Controllers
{
    [Route("api/[controller]")]
    [Route("api/[controller]/{fileId}")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly NotesDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SubscriptionController(NotesDbContext db,
            UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor
            )
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Get subscriptions for current user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<Subscription>> Get()
        {
            // Who am I?
            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser me = await _userManager.FindByIdAsync(userId);

            // my list
            List<Subscription> mine = await _db.Subscription.Where(p => p.SubscriberId == me.Id).ToListAsync();

            if (mine is null)
                mine = new List<Subscription>();

            List<Subscription> avail = new List<Subscription>();

            foreach (Subscription m in mine)
            {
                NoteAccess na = await AccessManager.GetAccess(_db, userId, m.NoteFileId, 0);
                if (na.ReadAccess)  // must have read access!!
                    avail.Add(m);
            }

            return avail;
        }

        [HttpPost]
        public async Task Post(SCheckModel model)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser me = await _userManager.FindByIdAsync(userId);

            int fileId = model.fileId;

            NoteFile file = _db.NoteFile.Find(fileId);

            Subscription sub = new Subscription
            {
                NoteFileId = fileId,
                NoteFile = file,
                SubscriberId = me.Id,
            };

            _db.Subscription.Add(sub);
            _db.Entry(sub).State = EntityState.Added;
            await _db.SaveChangesAsync();
        }

        [HttpDelete]
        public async Task Delete(int fileId)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser me = await _userManager.FindByIdAsync(userId);
            Subscription mine = await _db.Subscription.SingleOrDefaultAsync(p => p.SubscriberId == me.Id && p.NoteFileId == fileId);
            if (mine is null)
                return;

            _db.Subscription.Remove(mine);
            await _db.SaveChangesAsync();
        }

    }
}
