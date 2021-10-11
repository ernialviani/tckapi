using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TicketingApi.Migrations
{
    public partial class config_fcm_signalr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_client_details_client_groups_ClientGroupId",
                table: "client_details");

            migrationBuilder.RenameColumn(
                name: "ClientGroupId",
                table: "client_details",
                newName: "client_group_id");

            migrationBuilder.RenameIndex(
                name: "IX_client_details_ClientGroupId",
                table: "client_details",
                newName: "IX_client_details_client_group_id");

            migrationBuilder.CreateTable(
                name: "notif_registers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    sender_id = table.Column<int>(type: "int", nullable: true),
                    fcm_token = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    os = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    os_version = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    browser = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    browser_version = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notif_registers", x => x.id);
                    table.ForeignKey(
                        name: "FK_notif_registers_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "signalr_connections",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    connection_id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    sender_id = table.Column<int>(type: "int", nullable: true),
                    connected = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Singnalr_connection", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "notifs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    notif_register_id = table.Column<int>(type: "int", nullable: false),
                    viewed = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    title = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    message = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    link = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ntf_type = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    NtfData = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifs", x => x.id);
                    table.ForeignKey(
                        name: "FK_notifs_notif_registers_notif_register_id",
                        column: x => x.notif_register_id,
                        principalTable: "notif_registers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_notif_registers_user_id",
                table: "notif_registers",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_notifs_notif_register_id",
                table: "notifs",
                column: "notif_register_id");

            migrationBuilder.AddForeignKey(
                name: "FK_client_details_client_groups_client_group_id",
                table: "client_details",
                column: "client_group_id",
                principalTable: "client_groups",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_client_details_client_groups_client_group_id",
                table: "client_details");

            migrationBuilder.DropTable(
                name: "notifs");

            migrationBuilder.DropTable(
                name: "signalr_connections");

            migrationBuilder.DropTable(
                name: "notif_registers");

            migrationBuilder.RenameColumn(
                name: "client_group_id",
                table: "client_details",
                newName: "ClientGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_client_details_client_group_id",
                table: "client_details",
                newName: "IX_client_details_ClientGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_client_details_client_groups_ClientGroupId",
                table: "client_details",
                column: "ClientGroupId",
                principalTable: "client_groups",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
