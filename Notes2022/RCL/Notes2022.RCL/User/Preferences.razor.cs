using Notes2022.Shared;
using System.Net.Http.Json;

namespace Notes2022.RCL.User
{
    public partial class Preferences
    {
        //[Parameter] public EventCallback<Message> OnMessage { get; set; }
        private UserData UserData { get; set; }

        //private List<TZone> TZones { get; set; }
        //private TZone MyZone { get; set; }
        private string currentText { get; set; }

        //private int myZid { get; set; }
        //private LocalModel stuff { get; set; }
        //private List<LocalModel> MyList { get; set; }
        private List<LocalModel2> MySizes { get; set; }

        private string pageSize { get; set; }

        //private string TimeZone { get; set; }

        protected override async Task OnInitializedAsync()
        {
            //TZones = await Http.GetJsonAsync<List<TZone>>("api/TimeZones/");
            //TZones = TZones.OrderBy(p => p.OffsetHours).ThenBy(p => p.OffsetMinutes).ToList();
            UserData = await DAL.GetUserData(Http);
            //MyZone = TZones.SingleOrDefault(p => p.Id == UserData.TimeZoneID);
            //myZid = UserData.TimeZoneID;
            pageSize = UserData.Ipref2.ToString();
            MySizes = new List<LocalModel2> { new LocalModel2("0", "All"), new LocalModel2("5"), new LocalModel2("10"), new LocalModel2("12"), new LocalModel2("20") };
            //TimeZone = TimeZoneInfo.Local.DisplayName;
            //int OHours = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Hours;
            //int OMinutes = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Minutes;
            //MyList = new List<LocalModel>();
            //foreach (TZone item in TZones)
            //{
            //    if (item.OffsetHours == OHours && item.OffsetMinutes == OMinutes)
            //    {
            //        LocalModel x = new LocalModel(item.Id.ToString(), item.Name + " - " + item.Abbreviation + " - " + item.Offset);
            //        MyList.Add(x);
            //    }
            //}
            //currentText = "Current Time Zone: " + MyZone.Name + " - " + MyZone.Abbreviation + " - " + MyZone.Offset;
            currentText = " ";
        }

        private async Task OnSubmit()
        {
            //UserData.TimeZoneID = myZid;
            UserData.Ipref2 = int.Parse(pageSize);
            //MyZone = TZones.SingleOrDefault(p => p.Id == UserData.TimeZoneID);
            //currentText = "Current Time Zone: " + MyZone.Name + " - " + MyZone.Abbreviation + " - " + MyZone.Offset;
            await DAL.UpdateUserData(Http, UserData);
            Navigation.NavigateTo("");
        }

        private async Task Cancel()
        {
            Navigation.NavigateTo("");
        }

        //public class LocalModel
        //{
        //    public LocalModel(string id, string name)
        //    {
        //        Id = id;
        //        Name = name;
        //    }
        //    public string Id { get; set; }
        //    public string Name { get; set; }
        //}
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