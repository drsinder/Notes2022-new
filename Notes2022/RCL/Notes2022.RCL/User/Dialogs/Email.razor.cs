using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;

namespace Notes2022.RCL.User.Dialogs
{
    public partial class Email
    {
        [CascadingParameter]
        public BlazoredModalInstance ModalInstance { get; set; }

        public string emailaddr { get; set; }

        private void Ok()
        {
            ModalInstance.CloseAsync(ModalResult.Ok(emailaddr));
        }

        private void Cancel()
        {
            ModalInstance.CancelAsync();
        }
    }
}