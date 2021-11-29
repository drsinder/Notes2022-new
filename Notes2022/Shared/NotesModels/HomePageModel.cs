/*--------------------------------------------------------------------------
**
**  Copyright © 2019, Dale Sinder
**
**  Name: HomePageModel.cs
**
**  Description:
**      Used for the root "/" of the app
**
**  This program is free software: you can redistribute it and/or modify
**  it under the terms of the GNU General Public License version 3 as
**  published by the Free Software Foundation.
**  
**  This program is distributed in the hope that it will be useful,
**  but WITHOUT ANY WARRANTY; without even the implied warranty of
**  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
**  GNU General Public License version 3 for more details.
**  
**  You should have received a copy of the GNU General Public License
**  version 3 along with this program in file "license-gpl-3.0.txt".
**  If not, see <http://www.gnu.org/licenses/gpl-3.0.txt>.
**
**--------------------------------------------------------------------------
*/

using System.Runtime.Serialization;

namespace Notes2022.Shared
{
    /// <summary>
    /// Packet of info used on Home page, admin page, and a couple more places
    /// </summary>
    [DataContract]
    public class HomePageModel
    {
        /// <summary>
        /// List of relevant notefiles
        /// </summary>
        [DataMember(Order = 1)]
        public List<NoteFile> NoteFiles { get; set; }

        /// <summary>
        /// List of access tokens
        /// </summary>
        [DataMember(Order = 2)]
        public List<NoteAccess> NoteAccesses { get; set; }

        /// <summary>
        /// Message to display on home page.  Comes from most recent note
        /// in file homepagemessages
        /// </summary>
        [DataMember(Order = 3)]
        public string Message { get; set; }

        /// <summary>
        /// Who am I?
        /// </summary>
        [DataMember(Order = 4)]
        public UserData UserData { get; set; }

        /// <summary>
        /// List of all users
        /// </summary>
        [DataMember(Order = 5)]
        public List<UserData> UserListData { get; set; }
    }
}
