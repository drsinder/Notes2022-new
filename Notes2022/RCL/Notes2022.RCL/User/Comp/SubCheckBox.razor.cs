using Microsoft.AspNetCore.Components;
using Notes2022.Shared;
using System.Net.Http.Json;

namespace Notes2022.RCL.User.Comp
{
    public partial class SubCheckBox
    {
        [Parameter]
        public int fileId { get; set; }

        [Parameter]
        public bool isChecked { get; set; }

        public SCheckModel Model { get; set; }
        protected override void OnParametersSet()
        {
            Model = new SCheckModel
            {
                isChecked = isChecked,
                fileId = fileId
            };
        }

        public async Task OnClick()
        {
            isChecked = !isChecked;

            if (isChecked) // create item
            {
                await DAL.CreateSubscription(Http, Model);
            }
            else // delete it
            {
                await DAL.DeleteSubscription(Http, fileId);
            }

            StateHasChanged();
        }
    }
}