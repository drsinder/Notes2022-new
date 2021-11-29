/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: Tags.cs
    **
    ** Description:
    **      Tags placed on a note by author
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
using System.Runtime.Serialization;

namespace Notes2022.Shared
{
    /// <summary>
    /// This class defines a table in the database.
    /// 
    /// Zero or more of these objects may be associated with each note.
    /// Defines a simple tag or set of tags for a note.
    /// 
    /// </summary>
    [DataContract]
    public class Tags
    {
        // The fileid the note belongs to
        [Required]
        [DataMember(Order = 1)]
        public int NoteFileId { get; set; }

        [Required]
        [DataMember(Order = 2)]
        public int ArchiveId { get; set; }
        [Required]
        [DataMember(Order = 3)]
        public long NoteHeaderId { get; set; }

        //[ForeignKey("NoteHeaderId")]
        //public NoteHeader? NoteHeader { get; set; }

        [Required]
        [StringLength(30)]
        [DataMember(Order = 4)]
        public string? Tag { get; set; }

        public override string? ToString()
        {
            return Tag;
        }

        public static string ListToString(List<Tags> list)
        {
            string s = string.Empty;
            if (list is null || list.Count < 1)
                return s;

            foreach (Tags tag in list)
            {
                s += tag.Tag + " ";
            }

            return s.TrimEnd(' ');
        }

        public static List<Tags> StringToList(string s)
        {
            List<Tags> list = new List<Tags>();

            if (string.IsNullOrEmpty(s) || s.Length < 1)
                return list;

            string[] tags = s.Split(',', ';', ' ');

            if (tags is null || tags.Length < 1)
                return list;

            foreach (string t in tags)
            {
                string r = t.Trim().ToLower();
                list.Add(new Tags() { Tag = r });
            }

            return list;
        }

        public static List<Tags> StringToList(string s, long hId, int fId, int arcId)
        {
            List<Tags> list = new List<Tags>();

            if (string.IsNullOrEmpty(s) || s.Length < 1)
                return list;

            string[] tags = s.Split(',', ';', ' ');

            if (tags is null || tags.Length < 1)
                return list;

            foreach (string t in tags)
            {
                string r = t.Trim().ToLower();
                list.Add(new Tags() { Tag = r, NoteHeaderId = hId, NoteFileId = fId, ArchiveId = arcId });
            }

            return list;
        }

        public static List<Tags> CloneForLink(List<Tags> inp)
        {
            if (inp is null)
                return null;

            List<Tags> outp = new List<Tags>();

            if (inp.Count == 0)
                return outp;

            foreach (Tags t in inp)
            {
                outp.Add(new Tags { Tag = t.Tag });
            }

            return outp;
        }
    }
}
