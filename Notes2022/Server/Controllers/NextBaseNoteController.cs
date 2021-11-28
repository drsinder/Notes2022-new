using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notes2022.Server.Data;
using Notes2022.Server.Models;
using Notes2022.Shared;

namespace Notes2022.Server.Controllers
{
    [Route("api/[controller]")]
    [Route("api/[controller]/{headerId:long}")]
    [ApiController]
    public class NextBaseNoteController : ControllerBase
    {
        private readonly NotesDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NextBaseNoteController(NotesDbContext db,
            UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor
          )
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<long> Get(long headerId)
        {
            long newId = 0;

            NoteHeader oh = _db.NoteHeader.SingleOrDefault(x => x.Id == headerId);
            NoteHeader nh = _db.NoteHeader.SingleOrDefault(p => p.NoteFileId == oh.NoteFileId && p.NoteOrdinal == oh.NoteOrdinal + 1 && p.ResponseOrdinal == 0 && p.Version == 0);

            if (nh != null)
                newId = nh.Id;

            return newId;
        }
    }
}
