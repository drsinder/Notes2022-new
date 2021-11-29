using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Notes2022.Server.Models
{
    /// <summary>
    /// Extentions to the base IdentityUser
    /// 
    /// Contains fields mirrored locally in UserData
    /// 
    /// These fields are accessed and edited there and then written back
    /// enmass.  By contrast the predefined field not seen here are
    /// almost always accessed via methods.  These methods create a Validation 
    /// Stamp for the predefined fields.  Tinker with those directly and
    /// you will probably make the user "Unusable".
    /// 
    /// </summary>
    public class ApplicationUser : IdentityUser
    {

        [Required]
        [Display(Name = "Display Name")]
        [StringLength(50)]
        [PersonalData]
        public string? DisplayName { get; set; }

        [PersonalData]
        public int TimeZoneID { get; set; }

        [PersonalData]
        public int Ipref0 { get; set; }

        [PersonalData]
        public int Ipref1 { get; set; }

        [PersonalData]
        public int Ipref2 { get; set; } // user choosen page size

        [PersonalData]
        public int Ipref3 { get; set; }

        [PersonalData]
        public int Ipref4 { get; set; }

        [PersonalData]
        public int Ipref5 { get; set; }

        [PersonalData]
        public int Ipref6 { get; set; }

        [PersonalData]
        public int Ipref7 { get; set; }

        [PersonalData]
        public int Ipref8 { get; set; }

        [PersonalData]
        public int Ipref9 { get; set; } // bits extend bool properties


        [PersonalData]
        public bool Pref0 { get; set; }

        [PersonalData]
        public bool Pref1 { get; set; } // false = use paged note index, true= scrolled

        [PersonalData]
        public bool Pref2 { get; set; } // use alternate editor

        [PersonalData]
        public bool Pref3 { get; set; } // show responses by default

        [PersonalData]
        public bool Pref4 { get; set; } // multiple expanded responses

        [PersonalData]
        public bool Pref5 { get; set; } // expanded responses

        [PersonalData]
        public bool Pref6 { get; set; } // alternate text editor

        [PersonalData]
        public bool Pref7 { get; set; } // show content when expanded

        [PersonalData]
        public bool Pref8 { get; set; }

        [PersonalData]
        public bool Pref9 { get; set; }


        //[Display(Name = "Style Preferences")]
        //[StringLength(7000)]
        //[PersonalData]
        //public string? MyStyle { get; set; }

        [StringLength(100)]
        [PersonalData]
        public string? MyGuid { get; set; }

    }
}