/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: SQLFiles.cs
    **
    ** Description:
    **      File info record and data record for uploaded files
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



using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notes2022.Shared
{
    /// <summary>
    /// This class defines a table in the database.
    /// 
    /// Not currently in use.
    /// 
    /// </summary>
    public class SQLFile
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FileId { get; set; }

        [Required]
        [StringLength(300)]
        public string? FileName { get; set; }

        [Required]
        [StringLength(100)]
        public string? ContentType { get; set; }

        [Required]
        [StringLength(300)]
        public string? Contributor { get; set; }

        public SQLFileContent? Content { get; set; }


        [StringLength(1000)]
        public string? Comments { get; set; }

    }

    /// <summary>
    /// This class defines a table in the database.
    /// 
    /// Not currently in use.
    /// 
    /// </summary>
    public class SQLFileContent
    {

        [Key]
        public long SQLFileId { get; set; }

        public SQLFile? SQLFile { get; set; }

        [Required]
        public byte[]? Content { get; set; }
    }
}
