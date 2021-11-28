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


namespace Notes2022.Server.Controllers
{
    [Route("api/[controller]")]
    [Route("api/[controller]/{fileId}")]
    [ApiController]
    public class SequencerEditController : ControllerBase
    {
        private readonly NotesDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SequencerEditController(NotesDbContext db,
            UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor
            )
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }


        [HttpPut]
        public async Task Put(Sequencer seq)
        {
            Sequencer modified = await _db.Sequencer.SingleAsync(p => p.UserId == seq.UserId && p.NoteFileId == seq.NoteFileId);

            modified.LastTime = seq.LastTime;
            modified.Ordinal = seq.Ordinal;

            _db.Entry(modified).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

    }
}
