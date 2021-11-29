/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: NoteDataManager.cs
    **
    ** Description:
    **     Lots of methods for dealing with the database
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
using Microsoft.EntityFrameworkCore;
using Notes2022.Server.Data;
using Notes2022.Server.Models;
using Notes2022.Shared;
using System.Security.Claims;
using SearchOption = Notes2022.Shared.SearchOption;

namespace Notes2022.Server
{
    public static class NoteDataManager
    {
        /// <summary>
        /// Create a NoteFile
        /// </summary>
        /// <param name="db">NotesDbContext</param>
        /// <param name="userManager">UserManager</param>
        /// <param name="userId">UserID of creator</param>
        /// <param name="name">NoteFile name</param>
        /// <param name="title">NoteFile title</param>
        /// <returns></returns>
        public static async Task<bool> CreateNoteFile(NotesDbContext db,
            UserManager<ApplicationUser> userManager,
            string userId, string name, string title)
        {
            var query = db.NoteFile.Where(p => p.NoteFileName == name);
            if (!query.Any())
            {
                NoteFile noteFile = new()
                {
                    NoteFileName = name,
                    NoteFileTitle = title,
                    Id = 0,
                    OwnerId = userId,
                    LastEdited = DateTime.Now.ToUniversalTime()
                };
                db.NoteFile.Add(noteFile);
                await db.SaveChangesAsync();

                NoteFile nf = await db.NoteFile
                    .Where(p => p.NoteFileName == noteFile.NoteFileName)
                    .FirstOrDefaultAsync();

                await AccessManager.CreateBaseEntries(db, userManager, userId, nf.Id);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Delete a NoteFile
        /// </summary>
        /// <param name="db">NotesDbContext</param>
        /// <param name="id">NoteFileID</param>
        /// <returns></returns>
        public static async Task<bool> DeleteNoteFile(NotesDbContext db, int id)
        {
            // Things to delete:
            // 1)  X Entries in NoteContent
            // 2)  X Entries in BaseNoteHeader
            // 3)  X Entries in Sequencer
            // 4)  X Entries in NoteAccesses
            // 5)  X Entries in Marks
            // 6)  X Entries in SearchView
            // 7)  1 Entry in NoteFile

            // The above (1 - 6) now done by Cascade Delete of NoteFile

            //List<NoteContent> nc = await _db.NoteContent
            //    .Where(p => p.NoteFileID == id)
            //    .ToListAsync();
            //List<BaseNoteHeader> bnh = await GetBaseNoteHeadersForFile(_db, id);
            //List<Sequencer> seq = await _db.Sequencer
            //.Where(p => p.NoteFileID == id)
            //.ToListAsync();
            //List<NoteAccess> na = await AccessManager.GetAccessListForFile(_db, id);
            //List<Mark> marks = await _db.Mark
            //    .Where(p => p.NoteFileID == id)
            //    .ToListAsync();
            //List<SearchView> sv = await _db.SearchView
            //    .Where(p => p.NoteFileID == id)
            //    .ToListAsync();

            //_db.NoteContent.RemoveRange(nc);
            //_db.BaseNoteHeader.RemoveRange(bnh);
            //_db.Sequencer.RemoveRange(seq);
            //_db.NoteAccess.RemoveRange(na);
            //_db.Mark.RemoveRange(marks);
            //_db.SearchView.RemoveRange(sv);

            NoteFile noteFile = await db.NoteFile
               .Where(p => p.Id == id)
               .FirstAsync();

            for (int arcId = 0; arcId <= noteFile.NumberArchives; arcId++)
            {
                List<NoteAccess> na = await AccessManager.GetAccessListForFile(db, id, arcId);
                db.NoteAccess.RemoveRange(na);
            }

            List<Subscription> subs = await db.Subscription
                .Where(p => p.NoteFileId == id)
                .ToListAsync();
            db.Subscription.RemoveRange(subs);

            db.NoteFile.Remove(noteFile);

            await db.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Archive a notefile - Bump the files NumberArchives -
        /// Set all current ArchiveId (=0) to NumberArchives for
        /// NoteHeader, NoteAccess, Tags
        /// </summary>
        /// <param name="_db"></param>
        /// <param name="noteFile"></param>
        public static void ArchiveNoteFile(NotesDbContext _db, NoteFile noteFile)
        {
            noteFile.NumberArchives++;
            _db.Update(noteFile);

            List<NoteHeader> nhl = _db.NoteHeader.Where(p => p.NoteFileId == noteFile.Id && p.ArchiveId == 0).ToList();

            foreach (NoteHeader nh in nhl)
            {
                nh.ArchiveId = noteFile.NumberArchives;
                _db.Update(nh);
            }

            List<NoteAccess> nal = _db.NoteAccess.Where(p => p.NoteFileId == noteFile.Id && p.ArchiveId == 0).ToList();
            foreach (NoteAccess na in nal)
            {
                na.ArchiveId = noteFile.NumberArchives;
            }
            _db.NoteAccess.AddRange(nal);

            List<Tags> ntl = _db.Tags.Where(p => p.NoteFileId == noteFile.Id && p.ArchiveId == 0).ToList();
            foreach (Tags nt in ntl)
            {
                nt.ArchiveId = noteFile.NumberArchives;
                _db.Update(nt);
            }

            _db.SaveChanges();
        }

        /// <summary>
        /// Create a new note
        /// </summary>
        /// <param name="db">Database</param>
        /// <param name="nh">NoteHeader for new note</param>
        /// <param name="body">Note content</param>
        /// <param name="tags">the tags</param>
        /// <param name="dMessage">Director message (hold over from when it was not in header...)</param>
        /// <param name="send">Should we send emails? / Is this an Imported note?</param>
        /// <param name="linked">Are we processing linked file?</param>
        /// <param name="editing">Are we editing?</param>
        /// <returns></returns>
        public static async Task<NoteHeader> CreateNote(NotesDbContext db, NoteHeader nh, string body, string tags, string dMessage, bool send, bool linked, bool editing = false)
        {
            long editingId = nh.Id;

            nh.Id = 0;
            if (nh.ResponseOrdinal == 0 && !editing)  // base note
            {
                // get the next available note Ordinal
                nh.NoteOrdinal = await NextBaseNoteOrdinal(db, nh.NoteFileId, nh.ArchiveId);
            }

            if (!linked && !editing)    // create a GUID for use over link
            {
                nh.LinkGuid = Guid.NewGuid().ToString();
            }

            if (!send) // indicates an import operation / adjust time to UCT / assume original was CST = UCT-06, so add 6 hours
            {
                int offset = 6;
                if (nh.LastEdited.IsDaylightSavingTime())
                    offset--;

                // throw in an added random time in ms
                Random rand = new();
                int ms = rand.Next(999);

                nh.LastEdited = nh.LastEdited.AddHours(offset).AddMilliseconds(ms);
                nh.CreateDate = nh.LastEdited;
                nh.ThreadLastEdited = nh.CreateDate;
            }

            NoteFile nf = await db.NoteFile
                .Where(p => p.Id == nh.NoteFileId)
                .FirstOrDefaultAsync();

            nf.LastEdited = nh.CreateDate;
            db.Entry(nf).State = EntityState.Modified;
            db.NoteHeader.Add(nh);
            await db.SaveChangesAsync();

            NoteHeader newHeader = nh;

            if (newHeader.ResponseOrdinal == 0) // base note
            {
                newHeader.BaseNoteId = newHeader.Id;
                db.Entry(newHeader).State = EntityState.Modified;

                if (editing)
                {
                    // update BaseNoteId for all responses
                    List<NoteHeader> rhl = db.NoteHeader.Where(p => p.BaseNoteId == editingId && p.ResponseOrdinal > 0).ToList();
                    foreach (NoteHeader ln in rhl)
                    {
                        ln.BaseNoteId = newHeader.Id;
                        db.Entry(ln).State = EntityState.Modified;
                    }
                }

                await db.SaveChangesAsync();
            }
            else    // response
            {
                NoteHeader baseNote = await db.NoteHeader
                    .Where(p => p.NoteFileId == newHeader.NoteFileId && p.ArchiveId == newHeader.ArchiveId && p.NoteOrdinal == newHeader.NoteOrdinal && p.ResponseOrdinal == 0)
                    .FirstOrDefaultAsync();

                newHeader.BaseNoteId = baseNote.Id;
                db.Entry(newHeader).State = EntityState.Modified;
                await db.SaveChangesAsync();

            }

            if (editing)
            {
                // update RefId
                List<NoteHeader> rhl = db.NoteHeader.Where(p => p.RefId == editingId).ToList();
                foreach (NoteHeader ln in rhl)
                {
                    ln.RefId = newHeader.Id;
                    db.Entry(ln).State = EntityState.Modified;
                }
                await db.SaveChangesAsync();
            }

            NoteContent newContent = new()
            {
                NoteHeaderId = newHeader.Id,
                NoteBody = body
            };
            db.NoteContent.Add(newContent);
            await db.SaveChangesAsync();

            // deal with tags

            if (tags is not null && tags.Length > 1)
            {
                var theTags = Tags.StringToList(tags, newHeader.Id, newHeader.NoteFileId, newHeader.ArchiveId);

                if (theTags.Count > 0)
                {
                    await db.Tags.AddRangeAsync(theTags);
                    await db.SaveChangesAsync();
                }
            }

            // Check for linked notefile(s)

            List<LinkedFile> links = await db.LinkedFile.Where(p => p.HomeFileId == newHeader.NoteFileId && p.SendTo).ToListAsync();

            if (linked || links is null || links.Count < 1)
            {

            }
            else
            {
                foreach (var link in links) // que up the linked notes
                {
                    if (link.SendTo)
                    {
                        LinkQueue q = new()
                        {
                            Activity = newHeader.ResponseOrdinal == 0 ? LinkAction.CreateBase : LinkAction.CreateResponse,
                            LinkGuid = newHeader.LinkGuid,
                            LinkedFileId = newHeader.NoteFileId,
                            BaseUri = link.RemoteBaseUri,
                            Secret = link.Secret
                        };

                        db.LinkQueue.Add(q);
                        await db.SaveChangesAsync();
                    }
                }
            }

            return newHeader;
        }

        /// <summary>
        /// Create a new Response - See CreateNote for params
        /// </summary>
        /// <param name="db"></param>
        /// <param name="nh"></param>
        /// <param name="body"></param>
        /// <param name="tags"></param>
        /// <param name="dMessage"></param>
        /// <param name="send"></param>
        /// <param name="linked"></param>
        /// <param name="editing"></param>
        /// <returns></returns>
        public static async Task<NoteHeader> CreateResponse(NotesDbContext db, NoteHeader nh, string body, string tags, string dMessage, bool send, bool linked, bool editing = false)
        {
            // do setup and call CreateNote
            NoteHeader mine = await GetBaseNoteHeader(db, nh.BaseNoteId);
            db.Entry(mine).State = EntityState.Unchanged;
            await db.SaveChangesAsync();

            mine.ThreadLastEdited = DateTime.Now.ToUniversalTime();
            mine.ResponseCount++;

            db.Entry(mine).State = EntityState.Modified;
            await db.SaveChangesAsync();

            nh.ResponseOrdinal = mine.ResponseCount;
            nh.NoteOrdinal = mine.NoteOrdinal;
            return await CreateNote(db, nh, body, tags, dMessage, send, linked, editing);
        }

        /// <summary>
        /// Delete a Note
        /// </summary>
        /// <param name="db">NotesDbContext</param>
        /// <param name="nc">NoteContent</param>
        /// <returns></returns>
        public static async Task DeleteNote(NotesDbContext _db, NoteHeader nh)
        {
            nh.IsDeleted = true;
            _db.Entry(nh).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            if (nh.ResponseOrdinal == 0 && nh.ResponseCount > 0)
            {
                // delete all responses
                for (int i = 1; i <= nh.ResponseCount; i++)
                {
                    NoteHeader rh = _db.NoteHeader.Single(p => p.ResponseOrdinal == i && p.Version == 0);
                    rh.IsDeleted = true;
                    _db.Entry(rh).State = EntityState.Modified;
                }
                await _db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Delete a linked note - NOT called now...
        /// </summary>
        /// <param name="db"></param>
        /// <param name="nh"></param>
        /// <returns></returns>
        public static async Task<string> DeleteLinked(NotesDbContext db, NoteHeader nh)
        {
            // Check for linked notefile(s)

            List<LinkedFile> links = await db.LinkedFile.Where(p => p.HomeFileId == nh.NoteFileId).ToListAsync();

            if (links is null || links.Count < 1)
            {

            }
            else
            {
                foreach (var link in links)
                {
                    if (link.SendTo)
                    {
                        LinkQueue q = new()
                        {
                            Activity = LinkAction.Delete,
                            LinkGuid = nh.LinkGuid,
                            LinkedFileId = nh.NoteFileId,
                            BaseUri = link.RemoteBaseUri,
                            Secret = link.Secret
                        };

                        db.LinkQueue.Add(q);
                        await db.SaveChangesAsync();
                    }
                }
            }

            return "Ok";
        }

        /// <summary>
        /// Edit a note - does setup to keep version history then creates a new note: CreateNote
        /// </summary>
        /// <param name="db"></param>
        /// <param name="userManager"></param>
        /// <param name="nh"></param>
        /// <param name="nc"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public static async Task<NoteHeader> EditNote(NotesDbContext db, UserManager<ApplicationUser> userManager, NoteHeader nh, NoteContent nc, string tags)
        {
            // this is for making the current version 0 a higher version and creating a new version 0
            // begin by getting the old header and setting version to 1 more than the highest existing version

            // clone nh
            NoteHeader dh = nh.Clone();

            int nvers = await db.NoteHeader.CountAsync(p => p.NoteFileId == dh.NoteFileId && p.NoteOrdinal == dh.NoteOrdinal
                && p.ResponseOrdinal == dh.ResponseOrdinal && p.ArchiveId == dh.ArchiveId);

            NoteHeader oh = await db.NoteHeader.SingleAsync(p => p.Id == dh.Id);     //.Where(p => p.Id == nh.Id);
            oh.Version = nvers;
            db.Entry(oh).State = EntityState.Modified;

            await db.SaveChangesAsync();

            dh.LastEdited = DateTime.Now.ToUniversalTime();

            // then create new note

            return await CreateNote(db, dh, nc.NoteBody, tags, nh.DirectorMessage, true, false, true);

            // below is for the old replacing edited notes

            //NoteHeader eHeader = await GetBaseNoteHeader(db, nh.Id);
            //eHeader.LastEdited = nh.LastEdited;
            //eHeader.ThreadLastEdited = nh.ThreadLastEdited;
            //eHeader.NoteSubject = nh.NoteSubject;
            //db.Entry(eHeader).State = EntityState.Modified;

            //NoteContent eContent = await GetNoteContent(db, nh.NoteFileId, nh.ArchiveId, nh.NoteOrdinal, nh.ResponseOrdinal);
            //eContent.NoteBody = nc.NoteBody;
            //eContent.DirectorMessage = nc.DirectorMessage;
            //db.Entry(eContent).State = EntityState.Modified;

            //List<Tags> oTags = await GetNoteTags(db, nh.NoteFileId, nh.ArchiveId, nh.NoteOrdinal, nh.ResponseOrdinal, 0);
            //db.Tags.RemoveRange(oTags);

            //db.UpdateRange(oTags);
            //db.Update(eHeader);
            //db.Update(eContent);

            //await db.SaveChangesAsync();

            //// deal with tags

            //if (tags is not null && tags.Length > 1)
            //{
            //    var theTags = Tags.StringToList(tags, eHeader.Id, eHeader.NoteFileId, eHeader.ArchiveId);

            //    if (theTags.Count > 0)
            //    {
            //        await db.Tags.AddRangeAsync(theTags);
            //        await db.SaveChangesAsync();
            //    }
            //}

            //// Check for linked notefile(s)

            //List<LinkedFile> links = await db.LinkedFile.Where(p => p.HomeFileId == eHeader.NoteFileId && p.SendTo).ToListAsync();

            //if (links is null || links.Count < 1)
            //{

            //}
            //else
            //{
            //    foreach (var link in links)
            //    {
            //        if (link.SendTo)
            //        {
            //            LinkQueue q = new LinkQueue
            //            {
            //                Activity = LinkAction.Edit,
            //                LinkGuid = eHeader.LinkGuid,
            //                LinkedFileId = eHeader.NoteFileId,
            //                BaseUri = link.RemoteBaseUri,
            //                Secret = link.Secret
            //            };

            //            db.LinkQueue.Add(q);
            //            await db.SaveChangesAsync();
            //        }
            //    }
            //}

            //return eHeader;
        }

        //public static async Task<NoteContent> GetNoteContent(NotesDbContext db, int nfid, int ArcId, int noteord, int respOrd)
        //{
        //    var header = await db.NoteHeader
        //        .Where(p => p.NoteFileId == nfid && p.ArchiveId == ArcId && p.NoteOrdinal == noteord && p.ResponseOrdinal == respOrd)
        //        .FirstAsync();

        //    if (header is null)
        //        return null;

        //    var content = await db.NoteContent
        //        .OfType<NoteContent>()
        //        .Where(p => p.NoteHeaderId == header.Id)
        //        .FirstAsync();

        //    //content.NoteHeader = null;

        //    return content;
        //}

        /// <summary>
        /// Copy user prefs from ApplicationUser to UserData entity
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static UserData GetUserData(ApplicationUser user)
        {
            UserData aux = new();

            aux.UserId = user.Id;
            aux.DisplayName = user.DisplayName;
            aux.Email = user.Email;
            aux.TimeZoneID = user.TimeZoneID;

            aux.Ipref0 = user.Ipref0;
            aux.Ipref1 = user.Ipref1;
            aux.Ipref2 = user.Ipref2;
            aux.Ipref3 = user.Ipref3;
            aux.Ipref4 = user.Ipref4;
            aux.Ipref5 = user.Ipref5;
            aux.Ipref6 = user.Ipref6;
            aux.Ipref7 = user.Ipref7;
            aux.Ipref8 = user.Ipref8;
            aux.Ipref9 = user.Ipref9;

            aux.Pref0 = user.Pref0;
            aux.Pref1 = user.Pref1;
            aux.Pref2 = user.Pref2;
            aux.Pref3 = user.Pref3;
            aux.Pref4 = user.Pref4;
            aux.Pref5 = user.Pref5;
            aux.Pref6 = user.Pref6;
            aux.Pref7 = user.Pref7;
            aux.Pref8 = user.Pref8;
            aux.Pref9 = user.Pref9;

            aux.MyGuid = user.MyGuid;

            return aux;
        }

        /// <summary>
        /// Put user data from UserData into ApplicationUser Entity
        /// </summary>
        /// <param name="aux"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static ApplicationUser PutUserData(ApplicationUser aux, UserData user)
        {

            aux.Id = user.UserId;
            //aux.DisplayName = user.DisplayName;
            //aux.Email = user.Email;
            aux.TimeZoneID = user.TimeZoneID;

            aux.Ipref0 = user.Ipref0;
            aux.Ipref1 = user.Ipref1;
            aux.Ipref2 = user.Ipref2;
            aux.Ipref3 = user.Ipref3;
            aux.Ipref4 = user.Ipref4;
            aux.Ipref5 = user.Ipref5;
            aux.Ipref6 = user.Ipref6;
            aux.Ipref7 = user.Ipref7;
            aux.Ipref8 = user.Ipref8;
            aux.Ipref9 = user.Ipref9;

            aux.Pref0 = user.Pref0;
            aux.Pref1 = user.Pref1;
            aux.Pref2 = user.Pref2;
            aux.Pref3 = user.Pref3;
            aux.Pref4 = user.Pref4;
            aux.Pref5 = user.Pref5;
            aux.Pref6 = user.Pref6;
            aux.Pref7 = user.Pref7;
            aux.Pref8 = user.Pref8;
            aux.Pref9 = user.Pref9;

            aux.MyGuid = user.MyGuid;

            return aux;
        }

        /// <summary>
        /// Write user data to Db
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="db"></param>
        /// <param name="userD"></param>
        public static void PutUserData(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, NotesDbContext db, UserData userD)
        {
            try
            {
                string userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                ApplicationUser user = userManager.FindByIdAsync(userId).GetAwaiter().GetResult();


                user = PutUserData(user, userD);

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string x = ex.Message;
            }
            return;
        }

        //public static async Task<Search> GetUserSearch(NotesDbContext db, string userid)
        //{
        //    return await db.Search
        //        .Where(p => p.UserId == userid)
        //        .FirstOrDefaultAsync();
        //}

        //public static async Task<NoteHeader> GetNoteById(NotesDbContext db, long noteid)
        //{
        //    return await db.NoteHeader
        //        .Include("NoteContent")
        //        .Include("Tags")
        //        .Where(p => p.Id == noteid)
        //        .FirstOrDefaultAsync();
        //}

        /// <summary>
        /// Get notefile object from a file name
        /// </summary>
        /// <param name="db"></param>
        /// <param name="fname"></param>
        /// <returns></returns>
        public static async Task<NoteFile> GetFileByName(NotesDbContext db, string fname)
        {
            return await db.NoteFile
                .Where(p => p.NoteFileName == fname)
                .FirstOrDefaultAsync();
        }

        //public static async Task<List<NoteFile>> GetNoteFilesOrderedByNameWithOwner(NotesDbContext db)
        //{
        //    return await db.NoteFile
        //        .Include(a => a.Owner)
        //        .OrderBy(p => p.NoteFileName)
        //        .ToListAsync();
        //}


        //public static async Task<NoteFile> GetFileByIdWithOwner(NotesDbContext db, int id)
        //{
        //    return await db.NoteFile
        //        .Include(a => a.Owner)
        //        .Where(p => p.Id == id)
        //        .FirstOrDefaultAsync();
        //}

        /// <summary>
        /// Get next available BaseNoteOrdinal
        /// </summary>
        /// <param name="db">NotesDbContext</param>
        /// <param name="noteFileId">NoteFileID</param>
        /// <returns></returns>
        public static async Task<int> NextBaseNoteOrdinal(NotesDbContext db, int noteFileId, int arcId)
        {
            IOrderedQueryable<NoteHeader> bnhq = GetBaseNoteHeaderByIdRev(db, noteFileId, arcId);

            if (bnhq is null || !bnhq.Any())
                return 1;

            NoteHeader bnh = await bnhq.FirstAsync();
            return bnh.NoteOrdinal + 1;
        }

        //public static async Task<long> GetNumberOfNotes(NotesDbContext db, int fileid, int arcId)
        //{
        //    List<NoteHeader> notes = await db.NoteHeader
        //                        .Where(p => p.NoteFileId == fileid && p.ArchiveId == arcId)
        //                        .ToListAsync();
        //    return notes.Count;
        //}

        //public static async Task<long> GetNumberOfBaseNotes(NotesDbContext db, int fileid, int arcId)
        //{
        //    List<NoteHeader> notes = await db.NoteHeader
        //                        .Where(p => p.Id == fileid && p.ArchiveId == arcId && p.ResponseOrdinal == 0)
        //                        .ToListAsync();
        //    return notes.Count;
        //}

        /// <summary>
        /// Get BaseNoteHeaders in reverse order - we only plan to look at the 
        /// first one/one with highest NoteOrdinal
        /// </summary>
        /// <param name="db"></param>
        /// <param name="noteFileId"></param>
        /// <returns></returns>
        private static IOrderedQueryable<NoteHeader> GetBaseNoteHeaderByIdRev(NotesDbContext db, int noteFileId, int arcId)
        {
            return db.NoteHeader
                            .Where(p => p.NoteFileId == noteFileId && p.ArchiveId == arcId && p.ResponseOrdinal == 0)
                            .OrderByDescending(p => p.NoteOrdinal);
        }

        /// <summary>
        /// Get notefile entity from its Id
        /// </summary>
        /// <param name="db"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<NoteFile> GetFileById(NotesDbContext db, int id)
        {
            return await db.NoteFile
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();
        }

        //public static async Task<NoteHeader> GetNoteHeader(NotesDbContext db, long id)
        //{
        //    return await db.NoteHeader
        //        .Where(p => p.Id == id)
        //        .FirstOrDefaultAsync();

        //}

        //public static async Task<List<NoteHeader>> GetBaseNoteHeaders(NotesDbContext db, int id, int arcId)
        //{
        //    return await db.NoteHeader
        //        .Where(p => p.NoteFileId == id && p.ArchiveId == arcId && p.ResponseOrdinal == 0)
        //        .OrderBy(p => p.NoteOrdinal)
        //        .ToListAsync();
        //}

        //public static async Task<List<NoteHeader>> GetBaseNoteAndResponses(NotesDbContext db, int nfid, int arcId, int noteord)
        //{
        //    return await db.NoteHeader
        //        .Include("NoteContent")
        //        .Include("Tags")
        //        .Where(p => p.NoteFileId == nfid && p.ArchiveId == arcId && p.NoteOrdinal == noteord)
        //        .ToListAsync();
        //}

        /// <summary>
        /// No Longer includes NoteFile but does include NoteContent
        /// </summary>
        /// <param name="db"></param>
        /// <param name="noteid"></param>
        /// <returns></returns>
        public static async Task<NoteHeader> GetNoteByIdWithFile(NotesDbContext db, long noteid)
        {
            return await db.NoteHeader
                .Include("NoteContent")
                //.Include("NoteFile")
                .Include("Tags")
                .Where(p => p.Id == noteid)
                .OrderBy((x => x.NoteOrdinal))
                .FirstOrDefaultAsync();

        }

        /// <summary>
        /// Get a NoteHeader given its Id
        /// </summary>
        /// <param name="db"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<NoteHeader> GetBaseNoteHeader(NotesDbContext db, long id)
        {
            NoteHeader nh = await db.NoteHeader
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            return await db.NoteHeader
                .Where(p => p.Id == nh.BaseNoteId)
                .FirstOrDefaultAsync();
        }

        //public static async Task<List<NoteHeader>> GetBaseNoteAndResponsesHeaders(NotesDbContext db, int nfid, int arcId, int noteord)
        //{
        //    return await db.NoteHeader
        //        .Where(p => p.NoteFileId == nfid && p.ArchiveId == arcId && p.NoteOrdinal == noteord)
        //        .ToListAsync();
        //}

        //public static async Task<List<Tags>> GetNoteTags(NotesDbContext db, int nfid, int arcId, int noteord, int respOrd, int dummy)
        //{
        //    var header = await db.NoteHeader
        //        .Where(p => p.NoteFileId == nfid && p.ArchiveId == arcId && p.NoteOrdinal == noteord && p.ResponseOrdinal == respOrd)
        //        .FirstAsync();

        //    if (header is null)
        //        return null;

        //    var tags = await db.Tags
        //        .Where(p => p.NoteHeaderId == header.Id)
        //        .ToListAsync();

        //    //foreach (var tag in tags)
        //    //{
        //    //    tag.NoteHeader = null;
        //    //}

        //    return tags;
        //}

        //public static async Task<bool> SendNotesAsync(ForwardViewModel fv, NotesDbContext db, IEmailSender emailSender,
        //        string email, string name, string Url)
        //{
        //    await emailSender.SendEmailAsync(fv.ToEmail, fv.NoteSubject,
        //        await MakeNoteForEmail(fv, db, email, name, Url));

        //    return true;
        //}

        //private static async Task<string> MakeNoteForEmail(ForwardViewModel fv, NotesDbContext db, string email, string name, string ProductionUrl)
        //{
        //    NoteHeader nc = await GetNoteByIdWithFile(db, fv.NoteID);

        //    if (!fv.hasstring || !fv.wholestring)
        //    {
        //        return "Forwarded by Notes 2022 - User: " + email + " / " + name
        //            + "<p>File: " + nc.NoteFile.NoteFileName + " - File Title: " + nc.NoteFile.NoteFileTitle + "</p><hr/>"
        //            + "<p>Author: " + nc.AuthorName + "  - Director Message: " + nc.NoteContent.DirectorMessage + "</p><p>"
        //            + "<p>Subject: " + nc.NoteSubject + "</p>"
        //            + nc.LastEdited.ToShortDateString() + " " + nc.LastEdited.ToShortTimeString() + " UTC" + "</p>"
        //            + nc.NoteContent.NoteBody
        //            + "<hr/>" + "<a href=\"" + ProductionUrl + "NoteDisplay/Display/" + fv.NoteID + "\" >Link to note</a>";
        //    }
        //    else
        //    {
        //        List<NoteHeader> bnhl = await GetBaseNoteHeadersForNote(db, nc.NoteFileId, nc.ArchiveId, nc.NoteOrdinal);
        //        NoteHeader bnh = bnhl[0];
        //        fv.NoteSubject = bnh.NoteSubject;
        //        List<NoteHeader> notes = await GetBaseNoteAndResponses(db, nc.NoteFileId, nc.ArchiveId, nc.NoteOrdinal);

        //        StringBuilder sb = new StringBuilder();
        //        sb.Append("Forwarded by Notes 2022 - User: " + email + " / " + name
        //            + "<p>\nFile: " + nc.NoteFile.NoteFileName + " - File Title: " + nc.NoteFile.NoteFileTitle + "</p>"
        //            + "<hr/>");

        //        for (int i = 0; i < notes.Count; i++)
        //        {
        //            if (i == 0)
        //            {
        //                sb.Append("<p>Base Note - " + (notes.Count - 1) + " Response(s)</p>");
        //            }
        //            else
        //            {
        //                sb.Append("<hr/><p>Response - " + notes[i].ResponseOrdinal + " of " + (notes.Count - 1) + "</p>");
        //            }
        //            sb.Append("<p>Author: " + notes[i].AuthorName + "  - Director Message: " + notes[i].NoteContent.DirectorMessage + "</p>");
        //            sb.Append("<p>Subject: " + notes[i].NoteSubject + "</p>");
        //            sb.Append("<p>" + notes[i].LastEdited.ToShortDateString() + " " + notes[i].LastEdited.ToShortTimeString() + " UTC" + " </p>");
        //            sb.Append(notes[i].NoteContent.NoteBody);
        //            sb.Append("<hr/>");
        //            sb.Append("<a href=\"");
        //            sb.Append(ProductionUrl + "NoteDisplay/Display/" + notes[i].Id + "\" >Link to note</a>");
        //        }

        //        return sb.ToString();
        //    }

        //}

        /// <summary>
        /// Get the BaseNoteHeader for a Note
        /// </summary>
        /// <param name="db">NotesDbContext</param>
        /// <param name="nfid">fileid</param>
        /// <param name="noteord"></param>
        /// <returns></returns>
        //public static async Task<List<NoteHeader>> GetBaseNoteHeadersForNote(NotesDbContext db, int nfid, int arcId, int noteord)
        //{
        //    return await db.NoteHeader
        //        .Where(p => p.NoteFileId == nfid && p.ArchiveId == arcId && p.NoteOrdinal == noteord && p.ResponseOrdinal == 0)
        //        .ToListAsync();
        //}

        //public static async Task<NoteHeader> GetBaseNoteHeaderForOrdinal(NotesDbContext db, int fileid, int arcId, int ord)
        //{
        //    return await db.NoteHeader
        //        .Where(p => p.NoteFileId == fileid && p.ArchiveId == arcId && p.NoteOrdinal == ord && p.ResponseOrdinal == 0)
        //        .FirstOrDefaultAsync();
        //}


        //public static async Task<NoteHeader> GetEditedNoteHeader(NotesDbContext db, NoteHeader edited)
        //{
        //    return await db.NoteHeader
        //        .Where(p => p.NoteFileId == edited.NoteFileId && p.ArchiveId == edited.ArchiveId && p.NoteOrdinal == edited.NoteOrdinal)
        //        .FirstOrDefaultAsync();
        //}

        /// <summary>
        /// Given a NoteContent Object and Response number get the response NoteID
        /// </summary>
        /// <param name="db"></param>
        /// <param name="nc"></param>
        /// <param name="resp"></param>
        /// <returns></returns>
        //public static async Task<long?> FindResponseId(NotesDbContext db, NoteHeader nc, int resp)
        //{
        //    NoteHeader content = await db.NoteHeader
        //        .Where(p => p.NoteFileId == nc.NoteFileId && p.ArchiveId == nc.ArchiveId && p.NoteOrdinal == nc.NoteOrdinal && p.ResponseOrdinal == resp)
        //        .FirstOrDefaultAsync();

        //    return content?.Id;
        //}

        //public static async Task<NoteFile> GetFileByIdWithHeaders(NotesDbContext db, int id, int arcId)
        //{
        //    NoteFile nf = await db.NoteFile
        //        .Where(p => p.Id == id)
        //        .FirstOrDefaultAsync();

        //    //nf.NoteHeaders = await db.NoteHeader.Where(p => p.NoteFileId == id && p.ArchiveId == arcId).ToListAsync();

        //    return nf;
        //}

        
        public static async Task<List<NoteHeader>> GetAllHeaders(NotesDbContext db, int id, int arcId)
        {
            return await db.NoteHeader.Where(p => p.NoteFileId == id && p.ArchiveId == arcId).ToListAsync();
        }

        /// <summary>
        /// Get the BaseNoteHeader in a given file with given ordinal
        /// </summary>
        /// <param name="db">NotesDbContext</param>
        /// <param name="fileId">fileid</param>
        /// <param name="noteOrd">noteordinal</param>
        /// <returns></returns>
        public static async Task<NoteHeader> GetBaseNoteHeader(NotesDbContext db, int fileId, int arcId, int noteOrd)
        {
            return await db.NoteHeader
                                .Where(p => p.NoteFileId == fileId && p.ArchiveId == arcId && p.NoteOrdinal == noteOrd && p.ResponseOrdinal == 0)
                                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get a list of all Notes in a string/thread
        /// </summary>
        /// <param name="db">NotesDbContext</param>
        /// <param name="fileId">fileid</param>
        /// <param name="noteOrd">NoteOrdinal - identifies the string/thread</param>
        /// <returns></returns>
        //private static async Task<List<NoteHeader>> GetNoteContentList(NotesDbContext db, int fileId, int arcId, int noteOrd)
        //{
        //    return await db.NoteHeader
        //        .Where(p => p.NoteFileId == fileId && p.ArchiveId == arcId && p.NoteOrdinal == noteOrd)
        //        .ToListAsync();
        //}

        //public static async Task<List<NoteHeader>> GetSearchResponseList(NotesDbContext db, Search start, int myRespOrdinal, NoteHeader bnh, SearchOption so)
        //{
        //    // First try responses
        //    if (so == SearchOption.Tag)
        //    {
        //        return await db.NoteHeader
        //            .Include("Tags")
        //            .Where(x => x.NoteFileId == start.NoteFileId && x.ArchiveId == start.ArchiveId && x.NoteOrdinal == bnh.NoteOrdinal && x.ResponseOrdinal > myRespOrdinal)
        //            .ToListAsync();

        //    }
        //    if (so == SearchOption.Content || so == SearchOption.DirMess)
        //    {
        //        return await db.NoteHeader
        //        .Include("NoteContent")
        //        .Where(x => x.NoteFileId == start.NoteFileId && x.ArchiveId == start.ArchiveId && x.NoteOrdinal == bnh.NoteOrdinal && x.ResponseOrdinal > myRespOrdinal)
        //        .ToListAsync();
        //    }

        //    return await db.NoteHeader
        //    .Where(x => x.NoteFileId == start.NoteFileId && x.ArchiveId == start.ArchiveId && x.NoteOrdinal == bnh.NoteOrdinal && x.ResponseOrdinal > myRespOrdinal)
        //    .ToListAsync();
        //}

        //public static async Task<List<NoteHeader>> GetSearchHeaders(NotesDbContext db, Search start, NoteHeader bnh, SearchOption so)
        //{
        //    if (so == SearchOption.Tag)
        //    {
        //        return await db.NoteHeader
        //        .Include("Tags")
        //        .Where(x => x.NoteFileId == start.NoteFileId && x.ArchiveId == start.ArchiveId && x.NoteOrdinal > bnh.NoteOrdinal)
        //        .ToListAsync();
        //    }
        //    if (so == SearchOption.Content || so == SearchOption.DirMess)
        //    {
        //        return await db.NoteHeader
        //        .Include("NoteContent")
        //        .Where(x => x.NoteFileId == start.NoteFileId && x.ArchiveId == start.ArchiveId && x.NoteOrdinal > bnh.NoteOrdinal)
        //        .ToListAsync();
        //    }
        //    return await db.NoteHeader
        //        .Where(x => x.NoteFileId == start.NoteFileId && x.ArchiveId == start.ArchiveId && x.NoteOrdinal > bnh.NoteOrdinal)
        //        .ToListAsync();

        //}

        //public static async Task<List<Sequencer>> GetSeqListForUser(NotesDbContext db, string userid)
        //{
        //    return await db.Sequencer
        //        .Where(x => x.UserId == userid)
        //        .OrderBy(x => x.Ordinal)
        //        .ToListAsync();
        //}

        //public static async Task<List<NoteHeader>> GetSbnh(NotesDbContext db, Sequencer myseqfile)
        //{
        //    return await db.NoteHeader
        //                    .Where(x => x.NoteFileId == myseqfile.NoteFileId && x.ArchiveId == 0
        //                    && x.ResponseOrdinal == 0
        //                    && x.ThreadLastEdited >= myseqfile.LastTime)
        //                    .OrderBy(x => x.NoteOrdinal)
        //                    .ToListAsync();
        //}

        //public static async Task<List<NoteHeader>> GetSeqHeader1(NotesDbContext db, Sequencer myseqfile, NoteHeader bnh)
        //{
        //    return await db.NoteHeader
        //        .Where(x => x.NoteFileId == myseqfile.NoteFileId && x.ArchiveId == 0
        //            && x.LastEdited >= myseqfile.LastTime && x.NoteOrdinal > bnh.NoteOrdinal && x.ResponseOrdinal == 0)
        //        .OrderBy(x => x.NoteOrdinal)
        //        .ToListAsync();
        //}

        //public static async Task<List<NoteHeader>> GetSeqHeader2(NotesDbContext db, Sequencer myseqfile)
        //{
        //    return await db.NoteHeader
        //        .Where(x => x.NoteFileId == myseqfile.NoteFileId && x.ArchiveId == 0 && x.LastEdited >= myseqfile.LastTime && x.ResponseOrdinal == 0)
        //        .OrderBy(x => x.NoteOrdinal)
        //        .ToListAsync();
        //}

        /// <summary>
        /// Get all the BaseNoteHeaders for a file
        /// </summary>
        /// <param name="db">NotesDbContext</param>
        /// <param name="nfid">fileid</param>
        /// <returns></returns>
        //public static async Task<List<NoteHeader>> GetBaseNoteHeadersForFile(NotesDbContext db, int nfid, int arcId)
        //{
        //    return await db.NoteHeader
        //        .Where(p => p.NoteFileId == nfid && p.ArchiveId == arcId && p.ResponseOrdinal == 0)
        //        .OrderBy(p => p.NoteOrdinal)
        //        .ToListAsync();
        //}

        //public static async Task<List<NoteHeader>> GetOrderedListOfResponses(NotesDbContext db, int nfid, NoteHeader bnh)
        //{
        //    return await db.NoteHeader
        //        .Include(m => m.NoteContent)
        //        .Include(m => m.Tags)
        //        .Where(p => p.NoteFileId == nfid && p.ArchiveId == bnh.ArchiveId && p.NoteOrdinal == bnh.NoteOrdinal && p.ResponseOrdinal > 0)
        //        .OrderBy(p => p.ResponseOrdinal)
        //        .ToListAsync();
        //}

        //public static async Task<NoteHeader> GetMarkedNote(NotesDbContext db, Mark mark)
        //{
        //    return await db.NoteHeader
        //        .Include(m => m.NoteContent)
        //        .Include(m => m.Tags)
        //        .Where(p => p.NoteFileId == mark.NoteFileId && p.ArchiveId == mark.ArchiveId && p.NoteOrdinal == mark.NoteOrdinal && p.ResponseOrdinal == mark.ResponseOrdinal)
        //        .FirstAsync();
        //}

        
        public static async Task<NoteHeader> GetBaseNoteHeaderById(NotesDbContext db, long id)
        {
            return await db.NoteHeader
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();
        }

        public static async Task<List<NoteFile>> GetNoteFilesOrderedByName(NotesDbContext db)
        {
            return await db.NoteFile
                .OrderBy(p => p.NoteFileName)
                .ToListAsync();
        }


    }

}
