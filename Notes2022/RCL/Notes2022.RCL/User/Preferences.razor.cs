using Notes2022.Shared;

namespace Notes2022.RCL.User
{
    public partial class Preferences
    {
        private UserData UserData { get; set; }

        private string currentText { get; set; }

        private List<LocalModel2> MySizes { get; set; }

        private string pageSize { get; set; }

        protected override async Task OnInitializedAsync()
        {
            UserData = await DAL.GetUserData(Http);
            pageSize = UserData.Ipref2.ToString();
            MySizes = new List<LocalModel2> { new LocalModel2("0", "All"), new LocalModel2("5"), new LocalModel2("10"), new LocalModel2("12"), new LocalModel2("20") };
            currentText = " ";
        }

        private async Task OnSubmit()
        {
            UserData.Ipref2 = int.Parse(pageSize);
            await DAL.UpdateUserData(Http, UserData);
            Navigation.NavigateTo("");
        }

        private async Task Cancel()
        {
            Navigation.NavigateTo("");
        }

        public class LocalModel2
        {
            public LocalModel2(string psize)
            {
                Psize = psize;
                Name = psize;
            }

            public LocalModel2(string psize, string name)
            {
                Psize = psize;
                Name = name;
            }

            public string Psize { get; set; }

            public string Name { get; set; }
        }
    }
}