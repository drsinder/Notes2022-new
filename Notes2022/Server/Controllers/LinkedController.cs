/*--------------------------------------------------------------------------
    **
    ** Copyright© 2022, Dale Sinder
    **
    ** Name: LinkedController.cs
    **
    ** Description:
    **      Linked file managment
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
    **  If not, see<http: //www.gnu.org/licenses/gpl-3.0.txt>.
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
    [Route("api/[controller]/{Id}")]
    [ApiController]
    public class LinkedController : ControllerBase
    {
        private readonly NotesDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public LinkedController(NotesDbContext db,
            UserManager<ApplicationUser> userManager
          )
        {
            _db = db;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<List<LinkedFile>> Get()
        {
            return await _db.LinkedFile.ToListAsync();
        }

        [HttpPost]
        public async Task Post(LinkedFile linkedFile)
        {
            _db.LinkedFile.Add(linkedFile);
            await _db.SaveChangesAsync();
        }

        [HttpPut]
        public async Task Put(LinkedFile linkedFile)
        {
            _db.Entry(linkedFile).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        [HttpDelete]
        public async Task Delete(string Id)
        {
            int myId = int.Parse(Id);
            LinkedFile myFile = await _db.LinkedFile.SingleOrDefaultAsync(p => p.Id == myId);
            _db.LinkedFile.Remove(myFile);
            await _db.SaveChangesAsync();
        }

    }
}