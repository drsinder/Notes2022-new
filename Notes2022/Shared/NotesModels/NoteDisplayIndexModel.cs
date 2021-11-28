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
    public class NoteDisplayIndexModel
    {
        public NoteFile noteFile { get; set; }
        public int ArcId { get; set; }
        public NoteAccess myAccess { get; set; }
        public bool isMarked { get; set; }
        public string rPath { get; set; }
        public string scroller { get; set; }
        public int ExpandOrdinal { get; set; }
        public List<NoteHeader> Notes { get; set; }
        public NoteHeader myHeader { get; set; }
        public TZone tZone { get; set; }
        public List<NoteHeader> AllNotes { get; set; }
        public string linkedText { get; set; }
        public string message { get; set; }
        public UserData UserData { get; set; }
        public List<Tags> Tags { get; set; }
    }

}
