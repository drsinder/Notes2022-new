using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notes2022.Server.Data;
using Notes2022.Server.Models;

namespace Notes2022.Server.Controllers
{
    [Route("api/[controller]/{notefile}/{uploadfile}")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly NotesDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ImportController(NotesDbContext db,
            UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor
            )
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<bool> Get(string notefile, string uploadfile)
        {
            Importer imp = new Importer();
            return await imp.Import(_db, Globals.ImportRoot + uploadfile, notefile);
        }
    }
}
