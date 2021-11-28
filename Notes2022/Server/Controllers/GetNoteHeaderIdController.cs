using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notes2022.Server.Data;
using Notes2022.Server.Models;
using Notes2022.Shared;
using System.Security.Claims;

namespace Notes2022.Server.Controllers
{
    [Route("api/[controller]")]
    [Route("api/[controller]/{notefileId:int}/{noteOrd:int}/{noteRespOrd:int}")]
    [ApiController]
    public class GetNoteHeaderIdController : ControllerBase
    {
        private readonly NotesDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetNoteHeaderIdController(NotesDbContext db,
            UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor
            )
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<long> Get(int notefileId, int noteOrd, int noteRespOrd)
        {
            long newId = 0;
            NoteHeader nh;

            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            if (!await _userManager.IsInRoleAsync(user, "User"))
                return 0;

            try
            {
                nh = _db.NoteHeader.Single(p => p.NoteFileId == notefileId && p.NoteOrdinal == noteOrd && p.ResponseOrdinal == noteRespOrd && p.Version == 0);
                if (nh == null && noteRespOrd > -1) // try next base note -- special case if noteOrd == 0 and ResponseOrd == 0  ==> get first base note in file
                {
                    nh = _db.NoteHeader.Single(p => p.NoteFileId == notefileId && p.NoteOrdinal == noteOrd + 1 && p.ResponseOrdinal == 0 && p.Version == 0);
                }
                else if (nh == null)    // try previous base note
                {
                    nh = _db.NoteHeader.Single(p => p.NoteFileId == notefileId && p.NoteOrdinal == noteOrd - 1 && p.ResponseOrdinal == 0 && p.Version == 0);
                }
            }
            catch (Exception e)
            { return 0; }

            if (nh != null)
                newId = nh.Id;

            return newId;
        }
    }
}
