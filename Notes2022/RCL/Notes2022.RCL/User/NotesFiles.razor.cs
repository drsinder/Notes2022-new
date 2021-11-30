/*--------------------------------------------------------------------------
    **
    **  Copyright © 2022, Dale Sinder
    **
    **  Name: NotesFiles.razor.cs
    **
    **  Description:
    **      Displays a list of note files
    **
    **  This program is free software: you can redistribute it and/or modify
    **  it under the terms of the GNU General Public License version 3 as
    **  published by the Free Software Foundation.
    **
    **  This program is distributed in the hope that it will be useful,
    **  but WITHOUT ANY WARRANTY; without even the implied warranty of
    **  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    **  GNU General Public License version 3 for more details.
    **
    **  You should have received a copy of the GNU General Public License
    **  version 3 along with this program in file "license-gpl-3.0.txt".
    **  If not, see <http://www.gnu.org/licenses/gpl-3.0.txt>.
    **
    **--------------------------------------------------------------------------*/
using Notes2022.Shared;
using Syncfusion.Blazor.Grids;
using System.Net.Http.Json;

namespace Notes2022.RCL.User
{
    /// <summary>
    /// Display list of notefiles
    /// </summary>
    public partial class NotesFiles
    {
        private List<NoteFile> Files { get; set; }

        private UserData UserData { get; set; }

        /// <summary>
        /// Set up and get data from server
        /// </summary>
        /// <returns></returns>
        protected override async Task OnParametersSetAsync()
        {
            await sessionStorage.SetItemAsync("ArcId", 0);
            await sessionStorage.SetItemAsync("IndexPage", 1);

            // grab data from server
            HomePageModel model = await DAL.GetHomePageData(Http);

            Files = model.NoteFiles;
            UserData = model.UserData;
            if (UserData.Ipref2 == 0)
                UserData.Ipref2 = 10;
        }

        protected void DisplayIt(RowSelectEventArgs<NoteFile> args)
        {
            Navigation.NavigateTo("noteindex/" + args.Data.Id);
        }
    }
}