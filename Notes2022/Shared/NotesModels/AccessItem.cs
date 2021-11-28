/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: AccessItem.cs
    **
    ** Description:
    **      USed for managing access
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
    public enum AccessX
    {
        ReadAccess,
        Respond,
        Write,
        SetTag,
        DeleteEdit,
        ViewAccess,
        EditAccess
    }
    public class AccessItem
    {
        public NoteAccess Item { get; set; }
        public AccessX which { get; set; }
        public bool isChecked { get; set; }
        public bool canEdit { get; set; }
    }

}
