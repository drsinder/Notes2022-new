using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notes2022.Server.Data;
using Notes2022.Server.Models;
using Notes2022.Shared;
using System.Security.Claims;

namespace Notes2022.Server.Controllers
{
    [Route("api/[controller]/{noteid:long}")]
    [ApiController]
    public class DeleteNoteController : ControllerBase
    {
        private readonly NotesDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteNoteController(NotesDbContext db,
            UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor
            )
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Marks the given note as deleted
        /// </summary>
        /// <param name="noteid">Id of note to delete</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task Delete(long noteid)
        {
            // Who am I and am I at least in a User Role?
            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            bool test = await _userManager.IsInRoleAsync(user, "User");
            if (!test)
                return;

            NoteHeader nh = _db.NoteHeader.Single(p => p.Id == noteid);
            await NoteDataManager.DeleteNote(_db, nh);
        }

    }
}
