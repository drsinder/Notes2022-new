using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.RichTextEditor;
using System.Timers;

namespace Notes2022.RCL.User.Dialogs
{
    /// <summary>
    /// This takes the provided PrintStuff string and puts it in a
    /// Syncfusion editor component for the user to see.  It then allows
    /// the user to press a button to print.  TO do the printing
    /// the built in print function of the Sysncfusion editor is
    /// used so printing does not have to be separately implemented.
    /// </summary>
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
            timer2.Stop();
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