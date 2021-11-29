using System.ComponentModel.DataAnnotations;

namespace Notes2022.Shared
{
    /// <summary>
    /// Info bundle used to forward a note/string via email
    /// </summary>
    public class ForwardViewModel
    {
        /// <summary>
        /// File involved
        /// </summary>
        public NoteFile NoteFile { get; set; }

        /// <summary>
        /// Id of note user wants
        /// </summary>
        public long NoteID { get; set; }

        /// <summary>
        /// File Id of file
        /// </summary>
        public int FileID { get; set; }

        /// <summary>
        /// Archive I of file
        /// </summary>
        public int ArcID { get; set; }

        /// <summary>
        /// Ordianal / note # involved
        /// </summary>
        public int NoteOrdinal { get; set; }

        /// <summary>
        /// Subject of note
        /// </summary>
        [Display(Name = "Subject")]
        public string NoteSubject { get; set; }

        /// <summary>
        /// FLag to send the whole string or just one note?
        /// </summary>
        [Display(Name = "Forward whole note string")]
        public bool wholestring { get; set; }

        /// <summary>
        /// Does this note have a string?
        /// </summary>
        public bool hasstring { get; set; }

        /// <summary>
        /// Is this user an Admin
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Is the Admin sending the Everyone?
        /// </summary>
        public bool toAllUsers { get; set; }

        /// <summary>
        /// Target email address
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name = "Forward to Email Address")]
        public string? ToEmail { get; set; }

    }
}
