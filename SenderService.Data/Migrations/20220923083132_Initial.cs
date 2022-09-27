using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NTB.SenderService.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MessageStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MessageType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Subjectable = table.Column<bool>(type: "bit", nullable: false),
                    Attachable = table.Column<bool>(type: "bit", nullable: false),
                    MaxLenght = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    Recipient = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AttachesRef = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    MessageId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_MessageStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "MessageStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_MessageType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "MessageType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "MessageStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Поставлено в очередь" },
                    { 2, "В процессе отправки" },
                    { 3, "Передано провайдеру" },
                    { 4, "Доставлено получателю" },
                    { 9, "Ошибка доставки" }
                });

            migrationBuilder.InsertData(
                table: "MessageType",
                columns: new[] { "Id", "Attachable", "MaxLenght", "Name", "Subjectable" },
                values: new object[] { 4, true, 4096, "telegram", false });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_StatusId",
                table: "Messages",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_TypeId",
                table: "Messages",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageStatus_Name",
                table: "MessageStatus",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MessageType_Name",
                table: "MessageType",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "MessageStatus");

            migrationBuilder.DropTable(
                name: "MessageType");
        }
    }
}
