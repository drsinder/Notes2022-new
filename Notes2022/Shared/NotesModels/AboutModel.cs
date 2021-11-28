/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: AboutModel.cs
    **
    ** Description:
    **      Data for About page
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
    **  If not, see<http://www.gnu.org/licenses/gpl-3.0.txt>.
    **
    **--------------------------------------------------------------------------*/

using System.Runtime.Serialization;
using System.ServiceModel;

namespace Notes2022.Shared
{
    [DataContract]
    public class AboutModel
    {
        [DataMember(Order = 1)] 
        public string PrimeAdminName { get; set; }
        [DataMember(Order = 2)] 
        public string PrimeAdminEmail { get; set; }
        [DataMember(Order = 3)] 
        public DateTime StartupDateTime { get; set; }
    }
}
