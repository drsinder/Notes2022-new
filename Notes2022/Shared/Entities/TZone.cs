/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: TZone.cs
    **
    ** Description:
    **      Time Zones of the world for user selection
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
    /// Obsolete with Blazor WASM
    /// </summary>
    [DataContract]
    public class TZone
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        // ReSharper disable once InconsistentNaming
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        [DataMember(Order = 2)]
        public string? Name { get; set; }

        [Required]
        [StringLength(10)]
        [DataMember(Order = 3)]
        public string? Abbreviation { get; set; }

        [Required]
        [DataMember(Order = 4)]
        public string? Offset { get; set; }

        [Required]
        [DataMember(Order = 5)]
        public int OffsetHours { get; set; }

        [Required]
        [DataMember(Order = 6)]
        public int OffsetMinutes { get; set; }

        public DateTime Local(DateTime dt)
        {
            return dt.AddHours(OffsetHours).AddMinutes(OffsetMinutes);
        }

        //public DateTime LocalBlazor(DateTime dt)
        //{
        //    int OHours = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Hours;
        //    int OMinutes = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Minutes;

        //    return dt.AddHours(OHours).AddMinutes(OMinutes);
        //}


        public DateTime Universal(DateTime dt)
        {
            return dt.AddHours(-OffsetHours).AddMinutes(-OffsetMinutes);
        }
    }

}
