/*--------------------------------------------------------------------------
**
**  Copyright © 2019, Dale Sinder
**
**  Name: NoteDisplayIndexModel.cs
**
**  Description:
**      Data for the Main notes file list/index
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

namespace Notes2022.Shared
{
    /// <summary>
    /// Data needed to display the main note index
    /// </summary>
    public class NoteDisplayIndexModel
    {
        /// <summary>
        /// The notefile we are viewing
        /// </summary>
        public NoteFile noteFile { get; set; }

        /// <summary>
        /// Which archive?
        /// </summary>
        public int ArcId { get; set; }

        /// <summary>
        /// Users access token on the file
        /// </summary>
        public NoteAccess myAccess { get; set; }

        /// <summary>
        /// Are there marked items for this user?
        /// </summary>
        public bool isMarked { get; set; }

        public string rPath { get; set; }
        public string scroller { get; set; }
        public int ExpandOrdinal { get; set; }

        /// <summary>
        /// List of base notes
        /// </summary>
        public List<NoteHeader> Notes { get; set; }

        /// <summary>
        /// My current header - not used right now
        /// </summary>
        public NoteHeader myHeader { get; set; }

        /// <summary>
        /// Users TimeZone Info - not needed for WASM
        /// </summary>
        public TZone tZone { get; set; }

        /// <summary>
        /// List of all notes in file
        /// </summary>
        public List<NoteHeader> AllNotes { get; set; }

        /// <summary>
        /// Set to indicate a file is linked
        /// </summary>
        public string linkedText { get; set; }

        /// <summary>
        /// Potential error message
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// UserData for the current user
        /// </summary>
        public UserData UserData { get; set; }

        /// <summary>
        /// All Tags in file - used for searches
        /// </summary>
        public List<Tags> Tags { get; set; }
    }

}
