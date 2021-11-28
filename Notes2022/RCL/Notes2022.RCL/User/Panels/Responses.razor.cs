/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: Responses.razor.cs
    **
    ** Description:
    **      Displays response headers (and content) inside the NoteIndex
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

using Microsoft.AspNetCore.Components;
using Notes2022.Shared;
using Syncfusion.Blazor.Grids;

namespace Notes2022.RCL.User.Panels
{
    public partial class Responses
    {
        [Parameter] public List<NoteHeader> Headers { get; set; }
        [Parameter] public bool ShowContentR { get; set; }
        [Parameter] public bool ExpandAllR { get; set; }

        public bool ShowContent { get; set; }
        public bool ExpandAll { get; set; }


        protected SfGrid<NoteHeader> sfGrid2 { get; set; }

        [Inject] NavigationManager Navigation { get; set; }
        public Responses()
        {
        }

        protected override async Task OnInitializedAsync()
        {
            ShowContent = ShowContentR;
            ExpandAll = ExpandAllR;
        }

        public void DataBoundHandler()
        {
            if (ExpandAll)
            {
                sfGrid2.ExpandAllDetailRowAsync();
            }
        }

        private async void ExpandAllChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            if (ExpandAll)
            {
                await sfGrid2.ExpandAllDetailRowAsync();
            }
            else
            {
                await sfGrid2.CollapseAllDetailRowAsync();
            }
        }
        protected void DisplayIt(RowSelectEventArgs<NoteHeader> args)
        {
            Navigation.NavigateTo("notedisplay/" + args.Data.Id);
        }
    }
}
