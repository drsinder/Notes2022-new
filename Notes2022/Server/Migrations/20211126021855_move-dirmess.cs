using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notes2022.Server.Migrations
{
    public partial class movedirmess : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NoteHeader_NoteFile_NoteFileId",
                table: "NoteHeader");

            migrationBuilder.DropColumn(
                name: "DirectorMessage",
                table: "NoteContent");

            migrationBuilder.AddColumn<string>(
                name: "DirectorMessage",
                table: "NoteHeader",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DirectorMessage",
                table: "NoteHeader");

            migrationBuilder.AddColumn<string>(
                name: "DirectorMessage",
                table: "NoteContent",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_NoteHeader_NoteFile_NoteFileId",
                table: "NoteHeader",
                column: "NoteFileId",
                principalTable: "NoteFile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
