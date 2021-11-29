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

    /// <summary>
    /// Used for editing an access token segment (one flag)
    /// </summary>
    public class AccessItem
    {
        /// <summary>
        /// The whole token
        /// </summary>
        public NoteAccess Item { get; set; }

        /// <summary>
        /// Indicates which segment we are dealing with
        /// </summary>
        public AccessX which { get; set; }

        /// <summary>
        /// Is it currently checked?
        /// </summary>
        public bool isChecked { get; set; }

        /// <summary>
        /// Can current user change it?
        /// </summary>
        public bool canEdit { get; set; }
    }

}
