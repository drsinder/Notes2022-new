/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: Mark.cs
    **
    ** Description:
    **      Mark a user has placed on a string for later output
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
using System.ComponentModel.DataAnnotations.Schema;


namespace Notes2022.Shared
{
    public class Mark
    {
        [Required]
        [Column(Order = 0)]
        [StringLength(450)]
        public string? UserId { get; set; }

        [Required]
        [Column(Order = 1)]
        public int NoteFileId { get; set; }

        [Required]
        [Column(Order = 2)]
        public int ArchiveId { get; set; }

        [Required]
        [Column(Order = 3)]
        public int MarkOrdinal { get; set; }

        [Required]
        public int NoteOrdinal { get; set; }

        [Required]
        public long NoteHeaderId { get; set; }

        [Required]
        public int ResponseOrdinal { get; set; }  // -1 == whole string, 0 base note only, > 0 Response

        [ForeignKey("NoteFileId")]
        public NoteFile? NoteFile { get; set; }
    }
}
