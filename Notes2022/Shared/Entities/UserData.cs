/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: UserData.cs
    **
    ** Description:
    **      User Preferences
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
    /// <summary>
    /// This class does NOT define a table in the database!
    /// 
    /// It is a local mirror of the extra data fields added to the
    /// ApplicationUser.
    /// 
    /// </summary>
    [DataContract]
    public class UserData
    {
        [Required]
        [Key]
        [StringLength(450)]
        [DataMember(Order = 1)]
        public string UserId { get; set; }

        //[Display(Name = "Display Name")]
        [StringLength(50)]
        [DataMember(Order = 2)]
        public string DisplayName { get; set; }

        public string DisplayName2
        {
            get { return DisplayName.Replace(" ", "_"); }
        }

        [StringLength(150)]
        [DataMember(Order = 3)]
        public string Email { get; set; }

        [DataMember(Order = 4)]
        public int TimeZoneID { get; set; }

        [DataMember(Order = 5)]
        public int Ipref0 { get; set; }

        [DataMember(Order = 6)]
        public int Ipref1 { get; set; }

        [DataMember(Order = 7)]
        public int Ipref2 { get; set; } // user choosen page size

        [DataMember(Order = 8)]
        public int Ipref3 { get; set; }

        [DataMember(Order = 9)]
        public int Ipref4 { get; set; }

        [DataMember(Order = 10)]
        public int Ipref5 { get; set; }

        [DataMember(Order = 11)]
        public int Ipref6 { get; set; }

        [DataMember(Order = 12)]
        public int Ipref7 { get; set; }

        [DataMember(Order = 13)]
        public int Ipref8 { get; set; }

        [DataMember(Order = 14)]
        public int Ipref9 { get; set; } // bits extend bool properties


        [DataMember(Order = 15)]
        public bool Pref0 { get; set; }

        [DataMember(Order = 16)]
        public bool Pref1 { get; set; } // false = use paged note index, true= scrolled

        [DataMember(Order = 17)]
        public bool Pref2 { get; set; } // use alternate editor - obsolete

        [DataMember(Order = 18)]
        public bool Pref3 { get; set; } // show responses by default

        [DataMember(Order = 19)]
        public bool Pref4 { get; set; } // multiple expanded responses

        [DataMember(Order = 20)]
        public bool Pref5 { get; set; } // expanded responses

        [DataMember(Order = 21)]
        public bool Pref6 { get; set; } // alternate text editor

        [DataMember(Order = 22)]
        public bool Pref7 { get; set; } // show content when expanded

        [DataMember(Order = 23)]
        public bool Pref8 { get; set; }

        [DataMember(Order = 24)]
        public bool Pref9 { get; set; }

        [StringLength(100)]
        [DataMember(Order = 25)]
        public string? MyGuid { get; set; }

    }
}
