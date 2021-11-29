using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notes2022.Server.Data;
using Notes2022.Shared;

namespace Notes2022.Server.Controllers
{
    [Route("api/[controller]")]
    [Route("api/[controller]/{NoteId:long}")]
    [ApiController]
    public class GetFIleIdForNoteIdController : ControllerBase
    {

        private readonly NotesDbContext _db;

        public GetFIleIdForNoteIdController(NotesDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Given a NoteId gets its FileId
        /// </summary>
        /// <param name="NoteId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<int> Get(long NoteId)
        {
            NoteHeader header = _db.NoteHeader.SingleOrDefault(p => p.Id == NoteId);
            return header.NoteFileId;
        }
    }
}
