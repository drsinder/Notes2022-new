using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notes2022.Server.Data;
using Notes2022.Server.Models;
using Notes2022.Shared;

namespace Notes2022.Server.Controllers
{
    [Route("api/[controller]/{fileid:int}/{ordinal:int}/{respordinal:int}/{arcid:int}")]
    [ApiController]
    public class VersionsController : ControllerBase
    {
        private readonly NotesDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VersionsController(NotesDbContext db,
            UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor
            )
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Get Version NoteHeader list for given note
        /// </summary>
        /// <param name="fileid"></param>
        /// <param name="ordinal"></param>
        /// <param name="respordinal"></param>
        /// <param name="arcid"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<NoteHeader>> Get(int fileid, int ordinal, int respordinal, int arcid)
        {
            List<NoteHeader> hl;

            hl = _db.NoteHeader.Where(p => p.NoteFileId == fileid && p.Version != 0
                    && p.NoteOrdinal == ordinal && p.ResponseOrdinal == respordinal && p.ArchiveId == arcid)
                .OrderBy(p => p.Version)
                .ToList();

            return hl;
        }


    }
}
