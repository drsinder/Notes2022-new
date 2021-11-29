/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: ExportViewModel.cs
    **
    ** Description:
    **      Used to communicate export specs
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
    /// <summary>
    /// Used for exporting
    /// </summary>
    public class ExportViewModel
    {
        /// <summary>
        /// Notefile we are exporting from
        /// </summary>
        public NoteFile NoteFile { get; set; }

        /// <summary>
        /// Possible non 0 archive
        /// </summary>
        public int ArchiveNumber { get; set; }

        /// <summary>
        /// Is it to be in html format?
        /// </summary>
        public bool isHtml { get; set; }

        /// <summary>
        /// Collapsible or "flat"
        /// </summary>
        public bool isCollapsible { get; set; }

        /// <summary>
        /// Direct output or destination collected via a dailog?
        /// </summary>
        public bool isDirectOutput { get; set; }

        //public bool isOnPage { get; set; }

        /// <summary>
        /// NoteOrdinal to export - 0 for all notes
        /// </summary>
        public int NoteOrdinal { get; set; }

        /// <summary>
        /// "Marks" to limit scope of notes exportes the a specific set
        /// selected by user "Marked"
        /// </summary>
        public List<Mark> Marks { get; set; }

        /// <summary>
        /// Email address if being mailed to someone
        /// </summary>
        public string Email { get; set; }

    }

}
