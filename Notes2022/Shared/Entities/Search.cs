/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: Search.cs
    **
    ** Description:
    **      Represents a user search. 
    **      Not saved on server in blazor but is in non-blazor version
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



using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Notes2022.Shared
{
    public enum SearchOption { Author, Title, Content, Tag, DirMess, TimeIsAfter, TimeIsBefore }

    /// <summary>
    /// Model for searching a notefile
    /// </summary>
    [DataContract]
    public class Search
    {
        // User doing the search
        [StringLength(450)]
        [DataMember(Order = 1)]
        public string? UserId { get; set; }

        // search specs Option
        [Display(Name = "Search By")]
        [DataMember(Order = 2)]
        public SearchOption Option { get; set; }

        // Text to search for
        [Display(Name = "Search Text")]
        [DataMember(Order = 3)]
        public string? Text { get; set; }

        // DateTime to compare to
        [Display(Name = "Search Date/Time")]
        [DataMember(Order = 4)]
        public DateTime Time { get; set; }

        // current/next info -- where we are in the search
        [Column(Order = 0)]
        [DataMember(Order = 5)]
        public int NoteFileId { get; set; }

        [Required]
        [Column(Order = 1)]
        [DataMember(Order = 6)]
        public int ArchiveId { get; set; }

        [Column(Order = 2)]
        [DataMember(Order = 7)]
        public int BaseOrdinal { get; set; }
        [Column(Order = 3)]
        [DataMember(Order = 8)]
        public int ResponseOrdinal { get; set; }
        [Column(Order = 4)]
        [DataMember(Order = 9)]
        public long NoteID { get; set; }

        //[ForeignKey("NoteFileId")]
        //public NoteFile? NoteFile { get; set; }

        // Makes a clone of the object.  Had to do this to avoid side effects.
        public static Search Clone(Search s)
        {
            Search cloned = new Search
            {
                BaseOrdinal = s.BaseOrdinal,
                NoteFileId = s.NoteFileId,
                NoteID = s.NoteID,
                Option = s.Option,
                ResponseOrdinal = s.ResponseOrdinal,
                Text = s.Text,
                Time = s.Time,
                UserId = s.UserId
            };
            return cloned;
        }

    }
}
