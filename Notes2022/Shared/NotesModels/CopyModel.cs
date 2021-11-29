namespace Notes2022.Shared
{
    /// <summary>
    /// Used for copying a note/string from one file to another
    /// </summary>
    public class CopyModel
    {
        /// <summary>
        /// Header of note to copy
        /// </summary>
        public NoteHeader Note { get; set; }

        /// <summary>
        /// Id of target file
        /// </summary>
        public int FileId { get; set; }

        /// <summary>
        /// If true copy whole string.  Otherwise just the note.
        /// </summary>
        public bool WholeString { get; set; }
        //public UserData UserData { get; set; }
    }
}
