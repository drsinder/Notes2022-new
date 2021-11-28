using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notes2022.Server.Migrations
{
    public partial class Version : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "NoteHeader",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "NoteHeader");
        }
    }
}
