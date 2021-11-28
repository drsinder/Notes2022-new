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
    public class CheckedUser
    {
        public IdentityRole theRole { get; set; }

        public bool isMember { get; set; }
    }

    public class EditUserViewModel
    {
        public UserData UserData { get; set; }
        public List<CheckedUser> RolesList { get; set; }
        public string HangfireLoc { get; set; }
    }

}
