/*--------------------------------------------------------------------------
    **
    ** Copyright© 2022, Dale Sinder
    **
    ** Name: SequencerController.cs
    **
    ** Description:
    **      Manage sequencer data
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
    **  If not, see <http://www.gnu.org/licenses/gpl-3.0.txt>.
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
    [Route("api/[controller]/{fileId}")]
    [ApiController]
    public class SequencerController : ControllerBase
    {
        private readonly NotesDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SequencerController(NotesDbContext db,
            UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor
            )
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<List<Sequencer>> Get()
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser me = await _userManager.FindByIdAsync(userId);

            List<Sequencer> mine = await _db.Sequencer.Where(p => p.UserId == me.Id).OrderBy(p => p.Ordinal).ThenBy(p => p.LastTime).ToListAsync();

            if (mine is null)
                mine = new List<Sequencer>();

            List<Sequencer> avail = new List<Sequencer>();

            foreach (Sequencer m in mine)
            {
                NoteAccess na = await AccessManager.GetAccess(_db, userId, m.NoteFileId, 0);
                if (na.ReadAccess)
                    avail.Add(m);
            }
            return avail.OrderBy(p => p.Ordinal).ToList();
        }

        [HttpPost]
        public async Task Post(SCheckModel model)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser me = await _userManager.FindByIdAsync(userId);

            List<Sequencer> mine = await _db.Sequencer.Where(p => p.UserId == me.Id).OrderByDescending(p => p.Ordinal).ToListAsync();

            int ord;
            if (mine is null || mine.Count == 0)
            {
                ord = 1;
            }
            else
            {
                ord = mine[0].Ordinal + 1;
            }

            Sequencer tracker = new Sequencer
            {
                Active = true,
                NoteFileId = model.fileId,
                LastTime = DateTime.Now.ToUniversalTime(),
                UserId = me.Id,
                Ordinal = ord,
                StartTime = DateTime.Now.ToUniversalTime()
            };

            _db.Sequencer.Add(tracker);
            await _db.SaveChangesAsync();
        }

        [HttpDelete]
        public async Task Delete(int fileId)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser me = await _userManager.FindByIdAsync(userId);
            Sequencer mine = await _db.Sequencer.SingleOrDefaultAsync(p => p.UserId == me.Id && p.NoteFileId == fileId);
            if (mine is null)
                return;

            _db.Sequencer.Remove(mine);
            await _db.SaveChangesAsync();
        }

        [HttpPut]
        public async Task Put(Sequencer seq)
        {
            Sequencer modified = await _db.Sequencer.SingleAsync(p => p.UserId == seq.UserId && p.NoteFileId == seq.NoteFileId);

            modified.Active = seq.Active;
            if (seq.Active)  // starting to seq
            {
                modified.StartTime = DateTime.Now.ToUniversalTime();
            }
            else
            {
                modified.LastTime = seq.StartTime;
            }

            _db.Entry(modified).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

    }
}
