using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Notes2022.Shared;
using SearchOption = Notes2022.Shared.SearchOption;

namespace Notes2022.RCL.User.Dialogs
{
    public partial class SearchDlg
    {
        [CascadingParameter] BlazoredModalInstance ModalInstance { get; set; }

        [Parameter] public TZone zone { get; set; }
        [Parameter] public string searchtype { get; set; }

        //string Message { get; set; }

        private int option { get; set; }
        private string text { get; set; }
        private DateTime theTime { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            option = 0;
            theTime = DateTime.Now.ToUniversalTime();
        }
        private void Searchfor()
        {
            Search target = new Search();
            switch (option)
            {
                case 1: target.Option = SearchOption.Author; break;
                case 2: target.Option = SearchOption.Title; break;
                case 3: target.Option = SearchOption.Content; break;
                case 4: target.Option = SearchOption.DirMess; break;
                case 5: target.Option = SearchOption.Tag; break;
                case 6: target.Option = SearchOption.TimeIsBefore; break;
                case 7: target.Option = SearchOption.TimeIsAfter; break;
                default: return;
            }

            target.Text = text;

            //theTime = zone.Universal(theTime);
            target.Time = theTime;

            ModalInstance.CloseAsync(ModalResult.Ok<Search>(target));
        }

        private void Cancel()
        {
            ModalInstance.CloseAsync(ModalResult.Cancel());
        }

    }
}