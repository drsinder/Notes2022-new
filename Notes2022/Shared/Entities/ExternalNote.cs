/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: ExternalNote.cs
    **
    ** Description:
    **      unused at this time.  was intended for interface to external editor
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



using System.ComponentModel.DataAnnotations;

namespace Notes2022.Shared
{
    public class ExternalNote
    {
        [StringLength(100)]
        [Key]
        public string? NoteGuid { get; set; }

        public int FileId { get; set; }

        public int ArchiveId { get; set; }

        public long BaseNoteId { get; set; }

        public long EditNoteId { get; set; }

        [StringLength(200)]
        public string? Heading { get; set; }

        [StringLength(200)]
        [Display(Name = "Subject")]
        public string? NoteSubject { get; set; }

        [StringLength(200)]
        [Display(Name = "Director Message")]
        public string? DirectorMessage { get; set; }

        [StringLength(200)]
        [Display(Name = "Tags")]
        public string? Tags { get; set; }

        [StringLength(100000)]
        [Display(Name = "Note")]
        public string? NoteBody { get; set; }

    }
}
