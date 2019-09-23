using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MobileBackend.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DBUser",
                columns: table => new
                {
                    UUID = table.Column<Guid>(nullable: false),
                    userName = table.Column<string>(nullable: true),
                    User = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DBUser", x => x.UUID);
                    table.ForeignKey(
                        name: "FK_DBUser_Notification_User",
                        column: x => x.User,
                        principalTable: "Notification",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegisteredUsers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserID = table.Column<int>(nullable: false),
                    NotificationInstrumentID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisteredUsers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "device",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Platform = table.Column<string>(nullable: true),
                    notificationHubRegistration = table.Column<string>(nullable: true),
                    device = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_device", x => x.ID);
                    table.ForeignKey(
                        name: "FK_device_DBUser_device",
                        column: x => x.device,
                        principalTable: "DBUser",
                        principalColumn: "UUID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Instrument",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UUID = table.Column<Guid>(nullable: false),
                    Instrument = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instrument", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Instrument_DBUser_Instrument",
                        column: x => x.Instrument,
                        principalTable: "DBUser",
                        principalColumn: "UUID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DBUser_User",
                table: "DBUser",
                column: "User");

            migrationBuilder.CreateIndex(
                name: "IX_device_device",
                table: "device",
                column: "device");

            migrationBuilder.CreateIndex(
                name: "IX_Instrument_Instrument",
                table: "Instrument",
                column: "Instrument");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "device");

            migrationBuilder.DropTable(
                name: "Instrument");

            migrationBuilder.DropTable(
                name: "RegisteredUsers");

            migrationBuilder.DropTable(
                name: "DBUser");
        }
    }
}
