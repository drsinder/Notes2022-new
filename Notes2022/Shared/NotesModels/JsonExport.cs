namespace Notes2022.Shared
{
    /// <summary>
    /// Contents to be formatted into a Json string representing a
    /// notefile's content
    /// Usage:
    /// 
    ///    JsonExport stuff = await DAL.GetExportJson(Http, nfid, 0);
    ///    var stringContent = new StringContent(JsonConvert.SerializeObject(stuff, Formatting.Indented), Encoding.UTF8, "application/json");
    ///    Stream ms0 = await stringContent.ReadAsStreamAsync();
    ///    MemoryStream ms = new MemoryStream();
    ///    await ms0.CopyToAsync(ms);
    ///
    /// </summary>
    public class JsonExport
    {
        /// <summary>
        /// The Notefile
        /// </summary>
        public NoteFile NoteFile { get; set; }

        /// <summary>
        /// NoteHeaders - inside are also the NoteContent and
        /// Tags.
        /// </summary>
        public List<NoteHeader> NoteHeaders { get; set; }
    }
}
