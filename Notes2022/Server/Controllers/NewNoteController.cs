/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: NewBaseNoteController.cs
    **
    ** Description:
    **      Manages notes
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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notes2022.Server.Data;
using Notes2022.Server.Models;
using Notes2022.Server.Services;
using Notes2022.Shared;
using System.Security.Claims;

namespace Notes2022.Server.Controllers
{
    [Route("api/[controller]")]
    [Route("api/[controller]/{fileId}")]
    [ApiController]
    public class NewNoteController : ControllerBase
    {
        private readonly NotesDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NewNoteController(NotesDbContext db,
            UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor
          )
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<NoteFile> Get(int fileId)
        {
            NoteFile nf = _db.NoteFile.Single(p => p.Id == fileId);
            return nf;
        }

        [HttpPost]
        public async Task Post(TextViewModel tvm)
        {
            if (tvm.MyNote is null)
                return;

            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            bool test = await _userManager.IsInRoleAsync(user, "User");
            if (!test)
                return;

            ApplicationUser me = await _userManager.FindByIdAsync(userId);

            DateTime now = DateTime.Now.ToUniversalTime();
            NoteHeader nheader = new()
            {
                LastEdited = now,
                ThreadLastEdited = now,
                CreateDate = now,
                NoteFileId = tvm.NoteFileID,
                AuthorName = me.DisplayName,
                AuthorID = me.Id,
                NoteSubject = tvm.MySubject,
                DirectorMessage = tvm.DirectorMessage,
                ResponseOrdinal = 0,
                ResponseCount = 0
            };

            NoteHeader created;

            if (tvm.BaseNoteHeaderID == 0)
            {
                created = await NoteDataManager.CreateNote(_db, nheader, tvm.MyNote, tvm.TagLine, tvm.DirectorMessage, true, false);
            }
            else
            {
                nheader.BaseNoteId = tvm.BaseNoteHeaderID;
                nheader.RefId = tvm.RefId;
                created = await NoteDataManager.CreateResponse(_db, nheader, tvm.MyNote, tvm.TagLine, tvm.DirectorMessage, true, false);
            }


            await ProcessLinkedNotes();

            await SendNewNoteToSubscribers(created);

        }

        [HttpPut]
        public async Task Put(TextViewModel tvm)
        {
            if (tvm.MyNote is null)
                return;

            // get old Noteheader
            NoteHeader nheader = await NoteDataManager.GetBaseNoteHeaderById(_db, tvm.NoteID);

            // upate header
            DateTime now = DateTime.Now.ToUniversalTime();
            nheader.NoteSubject = tvm.MySubject;
            nheader.DirectorMessage = tvm.DirectorMessage;
            //nheader.LastEdited = now;
            nheader.ThreadLastEdited = now;

            NoteContent nc = new()
            {
                NoteHeaderId = tvm.NoteID,
                NoteBody = tvm.MyNote
            };

            await NoteDataManager.EditNote(_db, _userManager, nheader, nc, tvm.TagLine);

            await ProcessLinkedNotes();

            return;
        }

        private async Task SendNewNoteToSubscribers(NoteHeader myNote)
        {
            List<Subscription> subs = await _db.Subscription
                .Where(p => p.NoteFileId == myNote.NoteFileId)
                .ToListAsync();

            if (subs is null || subs.Count == 0)
                return;

            ForwardViewModel fv = new ForwardViewModel();
            fv.NoteID = myNote.Id;

            fv.NoteFile = await _db.NoteFile.SingleAsync(p => p.Id == myNote.NoteFileId);

            string myEmail = await LocalService.MakeNoteForEmail(fv, fv.NoteFile, _db, Globals.PrimeAdminEmail, Globals.PrimeAdminName);

            EmailSender emailSender = new EmailSender();

            foreach (Subscription s in subs)
            {
                ApplicationUser usr = await _userManager.FindByIdAsync(s.SubscriberId);

                NoteAccess na = await AccessManager.GetAccess(_db, usr.Id, s.NoteFileId, 0);
                if (na.ReadAccess)
                    BackgroundJob.Enqueue(() => emailSender.SendEmailAsync(usr.UserName, myNote.NoteSubject, myEmail));
            }
        }

        private async Task ProcessLinkedNotes()
        {
            List<LinkQueue> items = await _db.LinkQueue.Where(p => p.Enqueued == false).ToListAsync();
            foreach (LinkQueue item in items)
            {
                LinkProcessor lp = new LinkProcessor(_db);
                BackgroundJob.Enqueue(() => lp.ProcessLinkAction(item.Id));
                item.Enqueued = true;
                _db.Update(item);
            }
            if (items.Count > 0)
                await _db.SaveChangesAsync();

        }
    }
}
