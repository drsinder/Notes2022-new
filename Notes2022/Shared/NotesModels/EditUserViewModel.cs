/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: EditUserViewModel.cs
    **
    ** Description:
    **      UserData + Roles list and membership
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

using Microsoft.AspNetCore.Identity;

namespace Notes2022.Shared
{
    /// <summary>
    /// A Role and a membership flag
    /// </summary>
    public class CheckedUser
    {
        public IdentityRole theRole { get; set; }

        public bool isMember { get; set; }
    }

    /// <summary>
    /// Model for editing a user role membership
    /// </summary>
    public class EditUserViewModel
    {
        /// <summary>
        /// User data object - who are we - what's our name
        /// </summary>
        public UserData UserData { get; set; }

        /// <summary>
        /// List of Roles/memberships
        /// </summary>
        public List<CheckedUser> RolesList { get; set; }

        /// <summary>
        /// Extra information hanging around -  points to hangfire dashboard
        /// </summary>
        public string HangfireLoc { get; set; }
    }

}
