/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: UserEditController.cs
    **
    ** Description:
    **      Manage user roles
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
    **  If not, see<http://www.gnu.org/licenses/gpl-3.0.txt>.
    **
    **--------------------------------------------------------------------------*/


using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notes2022.Server.Data;
using Notes2022.Server.Models;
using Notes2022.Shared;

namespace Notes2022.Server.Controllers
{
    [Route("api/[controller]")]
    [Route("api/[controller]/{Id}")]
    [ApiController]
    public class UserEditController : ControllerBase
    {
        private readonly NotesDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserEditController(NotesDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<EditUserViewModel> Get(string Id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(Id);

            UserData me = NoteDataManager.GetUserData(user);

            var myRoles = await _userManager.GetRolesAsync(user);

            List<IdentityRole> allRoles = _db.Roles.OrderBy(p => p.Name).ToList();

            List<CheckedUser> myList = new List<CheckedUser>();

            foreach (IdentityRole item in allRoles)
            {
                CheckedUser it = new CheckedUser();
                it.theRole = item;
                it.isMember = myRoles.Where(p => p == item.Name).FirstOrDefault() is not null;
                myList.Add(it);
            }

            EditUserViewModel stuff = new EditUserViewModel()
            {
                UserData = me,
                RolesList = myList,
                HangfireLoc = Globals.HangfireLoc
            };

            return stuff;
        }

        [HttpPut]
        public async Task Put(EditUserViewModel model)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(model.UserData.UserId);
            var myRoles = await _userManager.GetRolesAsync(user);
            foreach (CheckedUser item in model.RolesList)
            {
                if (item.isMember && !myRoles.Contains(item.theRole.Name)) // need to add role
                {
                    await _userManager.AddToRoleAsync(user, item.theRole.Name);
                }
                else if (!item.isMember && myRoles.Contains(item.theRole.Name)) // need to remove role
                {
                    await _userManager.RemoveFromRoleAsync(user, item.theRole.Name);
                }
            }
        }

    }
}