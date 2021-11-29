/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: Sequencer.cs
    **
    ** Description:
    **      Used for tracking recent notes
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
using System.Runtime.Serialization;

namespace Notes2022.Shared
{
    /// <summary>
    /// This class defines a table in the database.
    /// 
    /// Object of this class may be associated with a user
    /// and file to be used to find notes written since the
    /// "Recent" function was last invoked.
    /// 
    /// </summary>
    [DataContract]
    public class Sequencer
    {
        // ID of the user who owns the item
        [Required]
        [Column(Order = 0)]
        [StringLength(450)]
        [DataMember(Order = 1)]
        public string? UserId { get; set; }

        // ID of target notfile
        [Required]
        [Column(Order = 1)]
        [DataMember(Order = 2)]
        public int NoteFileId { get; set; }

        [Required]
        [Display(Name = "Position in List")]
        [DataMember(Order = 3)]
        public int Ordinal { get; set; }

        // Time we last completed a run with this
        [Display(Name = "Last Time")]
        [DataMember(Order = 4)]
        public DateTime LastTime { get; set; }

        // Time a run in this file started - will get copied to LastTime when complete
        [DataMember(Order = 5)]
        public DateTime StartTime { get; set; }

        // Is this item active now?  Are we sequencing this file
        [DataMember(Order = 6)]
        public bool Active { get; set; }

        //[ForeignKey("NoteFileId")]
        //public NoteFile? NoteFile { get; set; }
    }
}
