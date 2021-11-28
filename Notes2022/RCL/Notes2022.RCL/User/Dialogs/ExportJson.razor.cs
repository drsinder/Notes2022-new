using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Notes2022.Shared;
using System.Net.Http.Json;
using System.Text;



namespace Notes2022.RCL.User.Dialogs
{
    public partial class ExportJson
    {
        [CascadingParameter] public BlazoredModalInstance ModalInstance { get; set; }
        [Parameter] public ExportViewModel model { get; set; }
        private string FileName { get; set; }

        private IJSObjectReference? module;

        private bool marked { get; set; }
        private string message = "Getting ready...";

        [Inject] HttpClient Http { get; set; }
        public ExportJson()
        {
        }

        protected async override Task OnInitializedAsync()
        {
            FileName = model.NoteFile.NoteFileName + ".json";

            if (model.Marks is not null && model.Marks.Count > 0)
                marked = true;
            else
                marked = false;

            message = "Exporting " + FileName;

            MemoryStream ms = await DoExport();

            await SaveAs(FileName, ms.GetBuffer());
            ms.Dispose();
            await ModalInstance.CancelAsync();
        }

        private async Task<MemoryStream> DoExport()
        {
            NoteFile nf = model.NoteFile;
            int nfid = nf.Id;
            JsonExport stuff = await DAL.GetExportJson(Http, nfid, 0);
            var stringContent = new StringContent(JsonConvert.SerializeObject(stuff, Formatting.Indented), Encoding.UTF8, "application/json");
            Stream ms0 = await stringContent.ReadAsStreamAsync();
            MemoryStream ms = new MemoryStream();
            await ms0.CopyToAsync(ms);
            ms0.Dispose();
            ms.Close();
            return ms;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                module = await JS.InvokeAsync<IJSObjectReference>("import",
                    "./scripts.js");
            }
        }


        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (module is not null)
            {
                await module.DisposeAsync();
            }
        }

        public async Task SaveAs(string filename, byte[] data)
        {
            await module.InvokeVoidAsync("saveAsFile", filename, Convert.ToBase64String(data));
        }


    }
}
