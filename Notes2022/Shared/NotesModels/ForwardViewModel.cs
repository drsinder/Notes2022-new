using System.ComponentModel.DataAnnotations;

namespace Notes2022.Shared
{
    public class ForwardViewModel
    {
        public NoteFile NoteFile { get; set; }
        public long NoteID { get; set; }
        public int FileID { get; set; }
        public int ArcID { get; set; }
        public int NoteOrdinal { get; set; }

        [Display(Name = "Subject")]
        public string NoteSubject { get; set; }

        [Display(Name = "Forward whole note string")]
        public bool wholestring { get; set; }

        public bool hasstring { get; set; }

        public bool IsAdmin { get; set; }

        public bool toAllUsers { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Forward to Email Address")]
        public string? ToEmail { get; set; }

    }
}
