/*--------------------------------------------------------------------------
    **
    ** Copyright© 2022, Dale Sinder
    **
    ** Name: ApiLinkController.cs
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

using Microsoft.AspNetCore.Mvc;
using Notes2022.Server.Data;
using Notes2022.Shared;

namespace Notes2022.Server.Controllers
{
    public class LinkCreateModel
    {
        public NoteHeader header { get; set; }

        public NoteContent content { get; set; }

        public List<Tags> tags { get; set; }

        public string linkedfile { get; set; }
        public string Secret { get; set; }
    }

    public class LinkCreateEModel
    {
        public NoteHeader header { get; set; }

        public NoteContent content { get; set; }

        public string tags { get; set; }

        public string linkedfile { get; set; }

        public string myGuid { get; set; }
        public string Secret { get; set; }
    }

    public class LinkCreateRModel
    {
        public NoteHeader header { get; set; }

        public NoteContent content { get; set; }

        public List<Tags> tags { get; set; }

        public string linkedfile { get; set; }

        public string baseGuid { get; set; }
        public string Secret { get; set; }

    }

    /// <summary>
    /// Has functions from former ApiLink, ApiLinkD, ApiLinkE Controllers
    /// </summary>

    [Route("api/[controller]")]
    [ApiController]
    public class ApiLinkController : ControllerBase
    {
        private readonly NotesDbContext _context;

        public ApiLinkController(NotesDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<string> CreateLinkNote(LinkCreateModel inputModel)
        {
            NoteFile file = _context.NoteFile.SingleOrDefault(p => p.NoteFileName == inputModel.linkedfile);

            if (file is null)
                return "Target file does not exist";

            // check for acceptance

            if (!await AccessManager.TestLinkAccess(_context, file, inputModel.Secret))
                return "Access Denied";

            inputModel.header.NoteFileId = file.Id;
            inputModel.header.ArchiveId = 0;
            inputModel.header.BaseNoteId = 0;
            inputModel.header.Id = 0;
            inputModel.header.NoteContent = null;
            //inputModel.header.NoteFile = null;
            inputModel.header.NoteOrdinal = 0;
            inputModel.header.ResponseOrdinal = 0;
            inputModel.header.ResponseCount = 0;

            var tags = Tags.ListToString(inputModel.tags);

            NoteHeader nh = await NoteDataManager.CreateNote(_context, inputModel.header,
                inputModel.content.NoteBody, tags, inputModel.header.DirectorMessage, true, true);

            if (nh is null)
            {

                return "Remote note create failed";
            }

            LinkLog ll = new LinkLog()
            {
                Event = "Ok",
                EventTime = DateTime.UtcNow,
                EventType = "RcvdCreateBaseNote"
            };

            _context.LinkLog.Add(ll);
            await _context.SaveChangesAsync();

            return "Ok";
        }

        [HttpPut]
        public async Task<string> EditLinkResponse(LinkCreateEModel inputModel)
        {
            NoteFile file = _context.NoteFile.SingleOrDefault(p => p.NoteFileName == inputModel.linkedfile);
            if (file is null)
                return "Target file does not exist";

            // check for acceptance

            if (!await AccessManager.TestLinkAccess(_context, file, inputModel.Secret))
                return "Access Denied";

            // find local base note for this and modify header

            NoteHeader extant = _context.NoteHeader.SingleOrDefault(p => p.LinkGuid == inputModel.myGuid);

            if (extant is null) // || extant.NoteFileId != file.Id)
                return "Could not find note";

            inputModel.header.NoteOrdinal = extant.NoteOrdinal;
            inputModel.header.ArchiveId = extant.ArchiveId;

            inputModel.header.NoteFileId = file.Id;

            inputModel.header.BaseNoteId = extant.BaseNoteId;
            inputModel.header.Id = extant.Id;
            //inputModel.header.NoteContent = null;
            //inputModel.header.NoteFile = null;
            inputModel.header.ResponseOrdinal = extant.ResponseOrdinal;
            inputModel.header.ResponseCount = extant.ResponseCount;


            NoteHeader nh = await NoteDataManager.EditNote(_context, null, inputModel.header,
                inputModel.content, inputModel.tags);

            if (nh is null)
            {

                return "Remote response edit failed";
            }

            return "Ok";
        }

        [HttpDelete]
        public async Task<string> DeleteLinkNote(string guid)
        {
            try
            {

                NoteHeader nh = _context.NoteHeader.SingleOrDefault(p => p.LinkGuid == guid);

                if (nh is null || nh.LinkGuid != guid)
                    return "No note to delete";

                try
                {
                    // check for acceptance

                    NoteFile file = _context.NoteFile.SingleOrDefault(p => p.Id == nh.NoteFileId);
                    if (file is null)
                        return "Target file does not exist";

                    if (!await AccessManager.TestLinkAccess(_context, file, string.Empty))
                        return "Access Denied";
                }
                catch
                { // ignore
                }

                await NoteDataManager.DeleteNote(_context, nh);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "Ok";
        }


        [HttpGet]
        public async Task<string> Test()
        {
            return "Hello Notes2022";
        }
    }
}
