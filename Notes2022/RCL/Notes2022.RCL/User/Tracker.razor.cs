/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: Tracker.razor.cs
    **
    ** Description:
    **      Sequencer / Tracker editor
    **
    ** This program is free software: you can redistribute it and/or modify
    ** it under the terms of the GNU General Public License version 3 as
    ** published by the Free Software Foundation.   
    **
    ** This program is distributed in the hope that it will be useful,
    ** but WITHOUT ANY WARRANTY; without even the implied warranty of
    ** MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
    ** GNU General Public License version 3 for more details.
    **
    **  You should have received a copy of the GNU General Public License
    **  version 3 along with this program in file "license-gpl-3.0.txt".
    **  If not, see <http://www.gnu.org/licenses/gpl-3.0.txt>.
    **
    **--------------------------------------------------------------------------*/

using Notes2022.Shared;

namespace Notes2022.RCL.User
{
    public partial class Tracker
    {
        private List<NoteFile> stuff { get; set; }

        private List<NoteFile> files { get; set; }

        private List<Sequencer> trackers { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            trackers = await DAL.GetSequencer(Http);
            HomePageModel model = await DAL.GetHomePageData(Http);
            stuff = model.NoteFiles.OrderBy(p => p.NoteFileName).ToList();
            await Shuffle();
        }

        public async Task Shuffle()
        {
            files = new List<NoteFile>();

            trackers = await DAL.GetSequencer(Http);
            if (trackers is not null)
            {
                trackers = trackers.OrderBy(p => p.Ordinal).ToList();
                foreach (var tracker in trackers)
                {
                    files.Add(stuff.Find(p => p.Id == tracker.NoteFileId));
                }
            }
            foreach (var s in stuff)
            {
                if (files.Find(p => p.Id == s.Id) is null)
                    files.Add(s);
            }
            StateHasChanged();
        }

        private async Task Cancel()
        {
            NavMan.NavigateTo("");
        }
    }
}