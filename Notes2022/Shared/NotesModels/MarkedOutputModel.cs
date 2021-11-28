/*--------------------------------------------------------------------------
**
**  Copyright © 2019, Dale Sinder
**
**  Name: MarkedOutputModel.cs
**
**  Description:
**      Specs for handing marked notes for output
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
**--------------------------------------------------------------------------
*/

namespace Notes2022.Shared
{

    public class MarkedOutputModel
    {
        public bool isHtml { get; set; }
        public bool isCollapsible { get; set; }
        public bool isEmail { get; set; }
        public string Email { get; set; }

    }

}

