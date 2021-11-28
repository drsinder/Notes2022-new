/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: EmailController.cs
    **
    ** Description:
    **      Send Email
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


using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Notes2022.Server.Data;
using Notes2022.Server.Services;
using Notes2022.Shared;

namespace Notes2022.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly NotesDbContext _db;

        public EmailController(NotesDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task Post(EmailModel stuff)
        {
            EmailSender sender = new EmailSender();

            //await sender.SendEmailAsync(stuff.email, stuff.subject, stuff.payload);

            BackgroundJob.Enqueue(() => sender.SendEmailAsync(stuff.email, stuff.subject, stuff.payload));
        }

    }
}