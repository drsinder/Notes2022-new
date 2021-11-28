using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Notes2022.Shared;
using System.Net.Http.Json;

namespace Notes2022.RCL.Admin.Dialogs
{
    public partial class UserEdit
    {
        [CascadingParameter] public BlazoredModalInstance ModalInstance { get; set; }

        [Parameter] public string UserId { get; set; }

        protected EditUserViewModel Model { get; set; }

        [Inject] HttpClient Http { get; set; }

        public UserEdit()
        {
        }

        protected override async Task OnParametersSetAsync()
        {
            Model = await DAL.GetUserEdit(Http, UserId);
        }

        private async Task Submit()
        {
            await DAL.UpdateUser(Http, Model);

            await ModalInstance.CancelAsync();
        }


        private async Task Done()
        {
            await ModalInstance.CancelAsync();
        }


    }
}