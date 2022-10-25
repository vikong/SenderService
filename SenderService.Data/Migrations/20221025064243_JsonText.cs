using Microsoft.EntityFrameworkCore.Migrations;

namespace NTB.SenderService.Data.Migrations
{
    public partial class JsonText : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachesRef",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Subject",
                table: "Messages");

            migrationBuilder.AddColumn<bool>(
                name: "IsJson",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsJson",
                table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "AttachesRef",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "Messages",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }
    }
}
