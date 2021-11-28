/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: LinkedFile.cs
    **
    ** Description:
    **      Represents a linked file
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
    public class LinkedFile
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [Required]
        [DataMember(Order = 2)]
        public int HomeFileId { get; set; }

        [Required]
        [StringLength(20)]
        [DataMember(Order = 3)]
        public string? HomeFileName { get; set; }

        [Required]
        [StringLength(20)]
        [DataMember(Order = 4)]
        public string? RemoteFileName { get; set; }

        [Required]
        [StringLength(450)]
        [DataMember(Order = 5)]
        public string? RemoteBaseUri { get; set; }

        [Required]
        [DataMember(Order = 6)]
        public bool AcceptFrom { get; set; }

        [Required]
        [DataMember(Order = 7)]
        public bool SendTo { get; set; }

        [StringLength(50)]
        [DataMember(Order = 8)]
        public string? Secret { get; set; }
    }
}
