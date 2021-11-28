using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.RichTextEditor;
using System.Text;
using System.Web;

namespace Notes2022.RCL.User.Dialogs
{
    public partial class CodeFormat
    {
        [CascadingParameter] BlazoredModalInstance ModalInstance { get; set; }
        [Parameter] public string stuff { get; set; }
        [Parameter] public SfRichTextEditor EditObj { get; set; }

        protected SfTextBox TextObj { get; set; }
        protected string message { get; set; }
        protected bool IsEditing { get; set; } = false;
        public string DropVal;

        public class CFormat
        {
            public string Name { get; set; }
            public string Code { get; set; }
        }

        public List<CFormat> CFormats = new List<CFormat>
        {
            new CFormat() { Name = "None",  Code = "none" },
            new CFormat() { Name = "C#",    Code = "csharp" },
            new CFormat() { Name = "C++",   Code = "cpp" },
            new CFormat() { Name = "C",     Code = "c" },
            new CFormat() { Name = "Razor", Code = "razor" },
            new CFormat() { Name = "Css",   Code = "css" },
            new CFormat() { Name = "Java",  Code = "java" },
            new CFormat() { Name = "JavaScript", Code = "js" },
            new CFormat() { Name = "Json",  Code = "json" },
            new CFormat() { Name = "Html",  Code = "html" },
            new CFormat() { Name = "Perl",  Code = "perl" },
            new CFormat() { Name = "Php",   Code = "php" },
            new CFormat() { Name = "SQL",   Code = "sql" },
            new CFormat() { Name = "PowerShell", Code = "powershell" }
        };

        protected async override Task OnParametersSetAsync()
        {
            message = stuff;
            IsEditing = !string.IsNullOrEmpty(stuff);   // not yet permitted at higher levels
        }

        private async Task Ok()
        {
            string code;

            if (DropVal is not null && !string.IsNullOrEmpty(DropVal))
                code = CFormats.Find(p => p.Name == DropVal).Code;
            else
                code = "none";

            switch (code)
            {
                case "none":
                    message = HttpUtility.HtmlEncode(TextObj.Value);
                    break;

                default:
                    message = HttpUtility.HtmlEncode(TextObj.Value);
                    message = MakeCode(message, code);
                    break;
            }
            //await EditObj.ExecuteCommandAsync(CommandName.InsertHTML, message);
            await ModalInstance.CloseAsync(ModalResult.Ok(message));
        }

        private string MakeCode(string stuff2, string codeType)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<pre><code class=\"language-");
            sb.Append(codeType);
            sb.Append("\">");
            sb.Append(stuff2);
            sb.Append("</code></pre>");

            return sb.ToString();
        }

        private void Cancel()
        {
            ModalInstance.CancelAsync();
        }
    }
}
