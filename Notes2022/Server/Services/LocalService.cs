/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: LocalService.cs
    **
    ** Description:
    **      Makes content from note to email
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


using Microsoft.EntityFrameworkCore;
using Notes2022.Server.Data;
using Notes2022.Shared;
using System.Text;

namespace Notes2022.Server.Services
{
    public static class LocalService
    {
        public static async Task<string> MakeNoteForEmail(ForwardViewModel fv, NoteFile NoteFile, NotesDbContext db, string email, string name)
        {
            NoteHeader nc = await NoteDataManager.GetNoteByIdWithFile(db, fv.NoteID);

            if (!fv.hasstring || !fv.wholestring)
            {
                return "Forwarded by Notes 2022 - User: " + email + " / " + name
                    + "<p>File: " + NoteFile.NoteFileName + " - File Title: " + NoteFile.NoteFileTitle + "</p><hr/>"
                    + "<p>Author: " + nc.AuthorName + "  - Director Message: " + nc.DirectorMessage + "</p><p>"
                    + "<p>Subject: " + nc.NoteSubject + "</p>"
                    + nc.LastEdited.ToShortDateString() + " " + nc.LastEdited.ToShortTimeString() + " UTC" + "</p>"
                    + nc.NoteContent.NoteBody
                    + "<hr/>" + "<a href=\"" + Globals.ProductionUrl + "/notedisplay/" + fv.NoteID + "\" >Link to note</a>";   // TODO
            }
            else
            {
                List<NoteHeader> bnhl = await db.NoteHeader
                    .Where(p => p.NoteFileId == nc.NoteFileId && p.NoteOrdinal == nc.NoteOrdinal && p.ResponseOrdinal == 0)
                    .ToListAsync();
                NoteHeader bnh = bnhl[0];
                fv.NoteSubject = bnh.NoteSubject;
                List<NoteHeader> notes = await db.NoteHeader.Include("NoteContent")
                    .Where(p => p.NoteFileId == nc.NoteFileId && p.NoteOrdinal == nc.NoteOrdinal)
                    .ToListAsync();

                StringBuilder sb = new();
                sb.Append("Forwarded by Notes 2022 - User: " + email + " / " + name
                    + "<p>\nFile: " + NoteFile.NoteFileName + " - File Title: " + NoteFile.NoteFileTitle + "</p>"
                    + "<hr/>");

                for (int i = 0; i < notes.Count; i++)
                {
                    if (i == 0)
                    {
                        sb.Append("<p>Base Note - " + (notes.Count - 1) + " Response(s)</p>");
                    }
                    else
                    {
                        sb.Append("<hr/><p>Response - " + notes[i].ResponseOrdinal + " of " + (notes.Count - 1) + "</p>");
                    }
                    sb.Append("<p>Author: " + notes[i].AuthorName + "  - Director Message: " + notes[i].DirectorMessage + "</p>");
                    sb.Append("<p>Subject: " + notes[i].NoteSubject + "</p>");
                    sb.Append("<p>" + notes[i].LastEdited.ToShortDateString() + " " + notes[i].LastEdited.ToShortTimeString() + " UTC" + " </p>");
                    sb.Append(notes[i].NoteContent.NoteBody);
                    sb.Append("<hr/>");
                    sb.Append("<a href=\"");
                    sb.Append(Globals.ProductionUrl + "/notedisplay/" + notes[i].Id + "\" >Link to note</a>");  // TODO
                }

                return sb.ToString();
            }
        }
    }
}
