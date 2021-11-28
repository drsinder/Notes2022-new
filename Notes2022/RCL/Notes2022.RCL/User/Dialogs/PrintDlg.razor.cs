using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.RichTextEditor;
using System.Timers;

namespace Notes2022.RCL.User.Dialogs
{
    public partial class PrintDlg
    {
        [CascadingParameter]
        BlazoredModalInstance ModalInstance { get; set; }

        [Parameter]
        public string PrintStuff { get; set; }

        private bool readonlyPrint { get; set; }

        SfRichTextEditor RteObj;
        private System.Timers.Timer timer2 { get; set; }

        private void onPrint()
        {
            RteObj.Print();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                timer2 = new System.Timers.Timer(500);
                timer2.Elapsed += TimerTick2;
                timer2.Enabled = true;
            }
        }

        protected void TimerTick2(Object source, ElapsedEventArgs e)
        {
            timer2.Enabled = false;
            readonlyPrint = false;
            this.RteObj.ExecuteCommand(CommandName.InsertHTML, PrintStuff);
            readonlyPrint = true;
            StateHasChanged();
        }

        private void ClosePrint()
        {
            ModalInstance.CancelAsync();
        }
    }
}