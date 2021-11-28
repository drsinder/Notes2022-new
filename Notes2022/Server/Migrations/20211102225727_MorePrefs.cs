using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notes2022.Server.Migrations
{
    public partial class MorePrefs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NoteAccess_NoteFile_NoteFileId",
                table: "NoteAccess");

            migrationBuilder.DropIndex(
                name: "IX_NoteAccess_NoteFileId",
                table: "NoteAccess");

            migrationBuilder.DropColumn(
                name: "MyStyle",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "Ipref0",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Ipref1",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Ipref9",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Pref0",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Pref9",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ipref0",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Ipref1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Ipref9",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Pref0",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Pref9",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "MyStyle",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                maxLength: 7000,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NoteAccess_NoteFileId",
                table: "NoteAccess",
                column: "NoteFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_NoteAccess_NoteFile_NoteFileId",
                table: "NoteAccess",
                column: "NoteFileId",
                principalTable: "NoteFile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
