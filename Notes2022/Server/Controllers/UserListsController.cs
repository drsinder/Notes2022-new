using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notes2022.Server.Data;
using Notes2022.Server.Models;
using Notes2022.Shared;

namespace Notes2022.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserListsController : ControllerBase
    {
        private readonly NotesDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;


        public UserListsController(NotesDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        /// <summary>
        /// Get List of all users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<UserData>> Get()
        {
            List<ApplicationUser> users = _db.Users.ToList();

            List<UserData> list = new List<UserData>();
            foreach (ApplicationUser user in users)
            {
                UserData userData = NoteDataManager.GetUserData(user);
                list.Add(userData);
            }

            return list;
        }

    }
}
