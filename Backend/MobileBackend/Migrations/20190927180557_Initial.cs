using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MobileBackend.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "login",
                columns: table => new
                {
                    key = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    userName = table.Column<string>(nullable: true),
                    encrypted = table.Column<byte[]>(nullable: true),
                    salt = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_login", x => x.key);
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
                name: "DBUser",
                columns: table => new
                {
                    UUID = table.Column<Guid>(nullable: false),
                    loginkey = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DBUser", x => x.UUID);
                    table.ForeignKey(
                        name: "FK_DBUser_login_loginkey",
                        column: x => x.loginkey,
                        principalTable: "login",
                        principalColumn: "key",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "device",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Platform = table.Column<string>(nullable: true),
                    notificationHubRegistration = table.Column<string>(nullable: true),
                    DeviceID = table.Column<Guid>(nullable: false),
                    LastContact = table.Column<DateTime>(nullable: false),
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
                    InstrumentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UUID = table.Column<Guid>(nullable: false),
                    Adress = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Instrument = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instrument", x => x.InstrumentID);
                    table.ForeignKey(
                        name: "FK_Instrument_DBUser_Instrument",
                        column: x => x.Instrument,
                        principalTable: "DBUser",
                        principalColumn: "UUID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    NotifivationID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    InstrumentID = table.Column<int>(nullable: false),
                    Instrument = table.Column<int>(nullable: true),
                    NotificationName = table.Column<string>(nullable: true),
                    NotificationDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.NotifivationID);
                    table.ForeignKey(
                        name: "FK_Notification_Instrument_Instrument",
                        column: x => x.Instrument,
                        principalTable: "Instrument",
                        principalColumn: "InstrumentID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RaisedNotification",
                columns: table => new
                {
                    NotificationId = table.Column<Guid>(nullable: false),
                    When = table.Column<DateTimeOffset>(nullable: false),
                    Instrument = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    InstrumentDisplayName = table.Column<string>(nullable: true),
                    DetailDescription = table.Column<string>(nullable: true),
                    notificationType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaisedNotification", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_RaisedNotification_Instrument_Instrument",
                        column: x => x.Instrument,
                        principalTable: "Instrument",
                        principalColumn: "InstrumentID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NotificationDBUser",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserUUID = table.Column<Guid>(nullable: false),
                    NotifivationID = table.Column<int>(nullable: false),
                    DBUserUUID = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationDBUser", x => x.ID);
                    table.ForeignKey(
                        name: "FK_NotificationDBUser_DBUser_DBUserUUID",
                        column: x => x.DBUserUUID,
                        principalTable: "DBUser",
                        principalColumn: "UUID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotificationDBUser_Notification_NotifivationID",
                        column: x => x.NotifivationID,
                        principalTable: "Notification",
                        principalColumn: "NotifivationID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Instrument",
                columns: new[] { "InstrumentID", "Adress", "DisplayName", "Instrument", "UUID" },
                values: new object[] { -1, "localhost", "VSPXXXXX", null, new Guid("ef091044-d994-404f-bb60-f4680107f27a") });

            migrationBuilder.InsertData(
                table: "Notification",
                columns: new[] { "NotifivationID", "Instrument", "InstrumentID", "NotificationDescription", "NotificationName" },
                values: new object[,]
                {
                    { -1, null, -1, "Instrument will cease to function without user attention", "Alert" },
                    { -2, null, -1, "Instrument will not function at full capacity without attention", "Warning" },
                    { -3, null, -1, "Batch will not complete without user attention", "ErrorSample" },
                    { -4, null, -1, "Inventory item below 30% capacity", "LowInventory" },
                    { -5, null, -1, "Inventory item below  5% capacity", "EmptyInventory" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DBUser_loginkey",
                table: "DBUser",
                column: "loginkey");

            migrationBuilder.CreateIndex(
                name: "IX_device_device",
                table: "device",
                column: "device");

            migrationBuilder.CreateIndex(
                name: "IX_Instrument_Instrument",
                table: "Instrument",
                column: "Instrument");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_Instrument",
                table: "Notification",
                column: "Instrument");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationDBUser_DBUserUUID",
                table: "NotificationDBUser",
                column: "DBUserUUID");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationDBUser_NotifivationID",
                table: "NotificationDBUser",
                column: "NotifivationID");

            migrationBuilder.CreateIndex(
                name: "IX_RaisedNotification_Instrument",
                table: "RaisedNotification",
                column: "Instrument");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "device");

            migrationBuilder.DropTable(
                name: "NotificationDBUser");

            migrationBuilder.DropTable(
                name: "RaisedNotification");

            migrationBuilder.DropTable(
                name: "RegisteredUsers");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "Instrument");

            migrationBuilder.DropTable(
                name: "DBUser");

            migrationBuilder.DropTable(
                name: "login");
        }
    }
}
