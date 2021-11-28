using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Notes2022.Shared;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Notes2022Preview.Pages.User
{
    public partial class FindFile
    {
        [Parameter] public string filename { get; set; }

        private HomePageModel? hpModel { get; set; }

        protected string message { get; set; }

        [Inject] HttpClient Http { get; set; }
        [Inject] AuthenticationStateProvider AuthProv { get; set; }
        [Inject] NavigationManager Navigation { get; set; }
        public FindFile()
        {
        }

        protected override async Task OnParametersSetAsync()
        {

            AuthenticationState authstate = await AuthProv.GetAuthenticationStateAsync();
            if (authstate.User.Identity.IsAuthenticated)
            {
                hpModel = await DAL.GetHomePageData(Http);

                NoteFile nf = hpModel.NoteFiles.SingleOrDefault(p => p.NoteFileName == filename);
                if (nf is not null)
                {
                    Navigation.NavigateTo("noteindex/" + nf.Id);
                }
                else
                {
                    message = "Note File '" + filename + "' not found...";
                }

            }

        }
    }
}
