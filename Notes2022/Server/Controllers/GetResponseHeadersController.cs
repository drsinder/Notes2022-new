using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notes2022.Server.Data;
using Notes2022.Server.Models;
using Notes2022.Shared;
using System.Security.Claims;

namespace Notes2022.Server.Controllers
{
    [Route("api/[controller]")]
    [Route("api/[controller]/{headerId:long}")]
    [ApiController]
    public class GetResponseHeadersController : ControllerBase
    {
        private readonly NotesDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetResponseHeadersController(NotesDbContext db,
            UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor
            )
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }


        [HttpGet]
        public async Task<List<NoteHeader>> Get(long headerId)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            bool test = await _userManager.IsInRoleAsync(user, "User");

            if (!test)
                return null;

            List<NoteHeader> result = _db.NoteHeader.Where(p => p.BaseNoteId == headerId && (p.ResponseOrdinal != 0) && p.IsDeleted == false && p.Version == 0).OrderBy(p => p.ResponseOrdinal).ToList();

            return result;


        }
    }
}
