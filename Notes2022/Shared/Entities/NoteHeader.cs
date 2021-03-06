/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: NoteHeader.cs
    **
    ** Description:
    **      Header for a note - every note, base or response has one
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
    /// <summary>
    /// This class defines a table in the database.
    /// NoteHeader objects are the high level descriptors for a note.
    /// They contain all the information about a note EXCEPT the
    /// body, which is contained in related class NoteContent.
    /// 
    /// The client index gets the complete set for a given notfile.
    /// This enables quick display, manipulation, and searching of
    /// the index.  Each object is related to one file object.
    /// 
    /// Fields:
    /// 
    /// Id          - The 64 bit unique Identifier for the note.
    /// NoteFileId  - The file which the note is a part of.
    /// ArchiveId   - 0 for the main file. Positive for archived notes.
    ///                 An Archive is a kind of subfile.
    /// BaseNoteId  - 0 for base notes. For responses the Id of its parent.
    /// NoteOrdinal - The number that appears in the index to Id a Base note.
    /// ResponseOrdinal - The number of a response. 0 for a base note.
    /// NoteSubject - You guessed it!
    /// LastEdited  - When the note was last edited
    /// ThreadLastEdited - When any note in a string was edited
    /// CreateDate  - DateTime when note was created
    /// ResponseCount - Only non-zero for a base note
    /// AuthorID    - UserId of the author
    /// AuthorName  - friendly name of author
    /// LinkGuid    - used to tie link notes together
    /// RefId       - Id of Note user was viewing when they responded
    /// IsDeleted   - true if the note has been marked as deleted
    /// Version     - Version Id for edited notes. Current is 0. Older are +
    /// DirectorMessage - Text message that may head a note.
    /// 
    /// A single NoteContent is associated with each NoteHeader.
    /// 
    /// </summary>
    [DataContract]
    public class NoteHeader
    {
        // Uniquely identifies the note
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataMember(Order = 1)]
        public long Id { get; set; }

        // The fileid the note belongs to
        [Required]
        [DataMember(Order = 2)]
        public int NoteFileId { get; set; }

        [Required]
        [DataMember(Order = 3)]
        public int ArchiveId { get; set; }

        [DataMember(Order = 4)]
        public long BaseNoteId { get; set; }

        // the ordinal on a Base note and all its responses
        [Required]
        [Display(Name = "Note #")]
        [DataMember(Order = 5)]
        public int NoteOrdinal { get; set; }

        // The ordinal of the response where 0 is a Base Note
        [Required]
        [Display(Name = "Response #")]
        [DataMember(Order = 6)]
        public int ResponseOrdinal { get; set; }

        // Subject/Title of a note
        [Required]
        [StringLength(200)]
        [Display(Name = "Subject")]
        [DataMember(Order = 7)]
        public string? NoteSubject { get; set; }

        // When the note was created or last edited
        [Required]
        [Display(Name = "Last Edited")]
        [DataMember(Order = 8)]
        public DateTime LastEdited { get; set; }

        // When the thread was last edited
        [Required]
        [Display(Name = "Thread Last Edited")]
        [DataMember(Order = 9)]
        public DateTime ThreadLastEdited { get; set; }

        [Required]
        [Display(Name = "Created")]
        [DataMember(Order = 10)]
        public DateTime CreateDate { get; set; }

        // Meaningful only if ResponseOrdinal = 0
        [Required]
        [DataMember(Order = 11)]
        public int ResponseCount { get; set; }

        // ReSharper disable once InconsistentNaming
        [StringLength(450)]
        [DataMember(Order = 12)]
        public string? AuthorID { get; set; }

        [Required]
        [StringLength(50)]
        [DataMember(Order = 13)]
        public string? AuthorName { get; set; }

        [StringLength(100)]
        [DataMember(Order = 14)]
        public string? LinkGuid { get; set; }

        [DataMember(Order = 15)]
        public long RefId { get; set; }

        [DataMember(Order = 16)]
        public bool IsDeleted { get; set; }

        [DataMember(Order = 17)]
        public int Version { get; set; }

        [StringLength(200)]
        [Display(Name = "Director Message")]
        [DataMember(Order = 18)]
        public string? DirectorMessage { get; set; }

        public NoteContent? NoteContent { get; set; }

        public List<Tags>? Tags { get; set; }


        public NoteHeader Clone()
        {
            NoteHeader nh = new NoteHeader()
            {
                Id = Id,
                NoteFileId = NoteFileId,
                ArchiveId = ArchiveId,
                BaseNoteId = BaseNoteId,
                NoteOrdinal = NoteOrdinal,
                NoteSubject = NoteSubject,
                DirectorMessage = DirectorMessage,
                LastEdited = LastEdited,
                ThreadLastEdited = ThreadLastEdited,
                CreateDate = CreateDate,
                ResponseCount = ResponseCount,
                AuthorID = AuthorID,
                AuthorName = AuthorName,
                LinkGuid = LinkGuid,
                RefId = RefId,
                IsDeleted = IsDeleted,
                Version = Version,
                ResponseOrdinal = ResponseOrdinal
            };

            return nh;
        }


        public NoteHeader CloneForLink()
        {
            NoteHeader nh = new NoteHeader()
            {
                Id = Id,
                NoteSubject = NoteSubject,
                DirectorMessage = DirectorMessage,
                LastEdited = LastEdited,
                ThreadLastEdited = ThreadLastEdited,
                CreateDate = CreateDate,
                AuthorID = AuthorID,
                AuthorName = AuthorName,
                LinkGuid = LinkGuid
            };

            return nh;
        }

        public NoteHeader CloneForLinkR()
        {
            NoteHeader nh = new NoteHeader()
            {
                Id = Id,
                NoteSubject = NoteSubject,
                DirectorMessage = DirectorMessage,
                LastEdited = LastEdited,
                ThreadLastEdited = ThreadLastEdited,
                CreateDate = CreateDate,
                AuthorID = AuthorID,
                AuthorName = AuthorName,
                LinkGuid = LinkGuid,
                ResponseOrdinal = ResponseOrdinal
            };

            return nh;
        }

        //public static explicit operator NoteHeader(Task<NoteHeader> v)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
