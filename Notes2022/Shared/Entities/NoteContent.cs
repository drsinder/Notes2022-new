/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: NoteContent.cs
    **
    ** Description:
    **      Note body and director message for note
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
using System.Runtime.Serialization;

namespace Notes2022.Shared
{
    [DataContract]
    public class NoteContent
    {
        [Required]
        [Key]
        [DataMember(Order = 1)]
        public long NoteHeaderId { get; set; }

        ////[ForeignKey("NoteHeaderId")]
        //public NoteHeader? NoteHeader { get; set; }

        // The Body or content of the note
        [Required]
        [StringLength(100000)]
        [Display(Name = "Note")]
        [DataMember(Order = 2)]
        public string? NoteBody { get; set; }

        // for imported notes compatability
        //[StringLength(200)]
        //[Display(Name = "Director Message")]
        //public string? DirectorMessage { get; set; }

        public NoteContent CloneForLink()
        {
            NoteContent nc = new NoteContent()
            {
                NoteBody = NoteBody,
                //DirectorMessage = DirectorMessage
            };

            return nc;
        }
    }
}
