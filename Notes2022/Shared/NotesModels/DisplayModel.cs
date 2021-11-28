/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: DisplayModel.cs
    **
    ** Description:
    **      NoteContent + tags
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

namespace Notes2022.Shared
{
    public class DisplayModel
    {
        public NoteFile noteFile { get; set; }
        public NoteHeader header { get; set; }
        public NoteContent content { get; set; }
        public List<Tags> tags { get; set; }
        public NoteAccess access { get; set; }
        public bool CanEdit { get; set; }
        public bool IsAdmin { get; set; }
    }
}
