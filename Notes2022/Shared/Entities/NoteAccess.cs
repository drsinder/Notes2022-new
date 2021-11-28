/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: NoteAccess.cs
    **
    ** Description:
    **      Access token for a file
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
    [DataContract]
    public class NoteAccess
    {
        [Required]
        [Column(Order = 0)]
        [StringLength(450)]
        [DataMember(Order = 1)]
        public string? UserID { get; set; }

        [Required]
        [Column(Order = 1)]
        [DataMember(Order = 2)]
        public int NoteFileId { get; set; }

        [Required]
        [Column(Order = 2)]
        [DataMember(Order = 3)]
        public int ArchiveId { get; set; }

        // Control options

        [Required]
        [Display(Name = "Read")]
        [DataMember(Order = 4)]
        public bool ReadAccess { get; set; }

        [Required]
        [Display(Name = "Respond")]
        [DataMember(Order = 5)]
        public bool Respond { get; set; }

        [Required]
        [Display(Name = "Write")]
        [DataMember(Order = 6)]
        public bool Write { get; set; }

        [Required]
        [Display(Name = "Set Tag")]
        [DataMember(Order = 7)]
        public bool SetTag { get; set; }

        [Required]
        [Display(Name = "Delete/Edit")]
        [DataMember(Order = 8)]
        public bool DeleteEdit { get; set; }

        [Required]
        [Display(Name = "View Access")]
        [DataMember(Order = 9)]
        public bool ViewAccess { get; set; }

        [Required]
        [Display(Name = "Edit Access")]
        [DataMember(Order = 10)]
        public bool EditAccess { get; set; }
    }
}
