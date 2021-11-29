/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: Globals.cs
    **
    ** Description:
    **      Adds one item to apps collection of shared global defines
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

using Notes2022.Shared;

namespace Notes2022.RCL
{
    public static class Globals
    {
        public static bool RolesValid { get; set; }
        public static bool IsAdmin { get; set; }
        public static bool IsUser { get; set; }
        public static EditUserViewModel? EditUserVModel { get; set; }

        public static UserData UserData { get; set; }

        //public static UserData GetUserData()
        //{
        //    return UserData;
        //}

        public static List<UserData> UserDataList { get; set; }

        public static DateTime StartupDateTime { get; set; }

        //public static string StuffUrl { get; } = "https://www.drsinder.com/NotesStuff/Notes2022/";

        //public static string AccessOther() { return "Other"; }

        public static string AccessOtherId { get; } = "Other";


        public static DateTime LocalTimeBlazor(DateTime dt)
        {
            int OHours = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Hours;
            int OMinutes = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Minutes;

            return dt.AddHours(OHours).AddMinutes(OMinutes);
        }
    }
}
