using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notes2022.Server.Migrations
{
    public partial class AddNoteDeletedFlag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "NoteHeader",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "NoteHeader");
        }
    }
}
