using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TicketingApi.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "apps",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    logo = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    desc = table.Column<string>(type: "nvarchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apps", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "clogs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    version = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    desc = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    app_id = table.Column<int>(type: "int", nullable: false),
                    module_id = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CLog", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "departments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    desc = table.Column<string>(type: "nvarchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Depatments", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "faqs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    question = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    desc = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    app_id = table.Column<int>(type: "int", nullable: false),
                    module_id = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faqs", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "kbases",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    body = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    app_id = table.Column<int>(type: "int", nullable: false),
                    module_id = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KBase", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    desc = table.Column<string>(type: "nvarchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "senders",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    first_name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    salt = table.Column<string>(type: "nvarchar(36)", nullable: false),
                    image = table.Column<string>(type: "nvarchar(150)", nullable: true),
                    color = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    login_status = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sender", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "stats",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    color = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    desc = table.Column<string>(type: "nvarchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stat", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    first_name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    salt = table.Column<string>(type: "nvarchar(36)", nullable: false),
                    image = table.Column<string>(type: "nvarchar(150)", nullable: true),
                    color = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    deleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "modules",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    desc = table.Column<string>(type: "nvarchar(150)", nullable: true),
                    app_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.id);
                    table.ForeignKey(
                        name: "FK_modules_apps_app_id",
                        column: x => x.app_id,
                        principalTable: "apps",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "teams",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    desc = table.Column<string>(type: "nvarchar(150)", nullable: true),
                    manager_id = table.Column<int>(type: "int", nullable: false),
                    image = table.Column<string>(type: "nvarchar(150)", nullable: true),
                    color = table.Column<string>(type: "nvarchar(45)", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(150)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    deleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.id);
                    table.ForeignKey(
                        name: "FK_teams_users_manager_id",
                        column: x => x.manager_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_depatments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    dept_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDepts", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_depatments_departments_dept_id",
                        column: x => x.dept_id,
                        principalTable: "departments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_depatments_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    role_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_roles_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_roles_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tickets",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ticket_number = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    subject = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    comment = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    app_id = table.Column<int>(type: "int", nullable: false),
                    module_id = table.Column<int>(type: "int", nullable: false),
                    sender_id = table.Column<int>(type: "int", nullable: true),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    stat_id = table.Column<int>(type: "int", nullable: false),
                    pending_by = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    pending_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    solved_by = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    solved_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    rejected_by = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    rejected_reason = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    rejected_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    ticket_type = table.Column<string>(type: "nvarchar(1)", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.id);
                    table.ForeignKey(
                        name: "FK_tickets_apps_app_id",
                        column: x => x.app_id,
                        principalTable: "apps",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tickets_modules_module_id",
                        column: x => x.module_id,
                        principalTable: "modules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tickets_senders_sender_id",
                        column: x => x.sender_id,
                        principalTable: "senders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tickets_stats_stat_id",
                        column: x => x.stat_id,
                        principalTable: "stats",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tickets_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "team_members",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    team_id = table.Column<int>(type: "int", nullable: false),
                    deleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    user_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team_Members", x => x.id);
                    table.ForeignKey(
                        name: "FK_team_members_teams_team_id",
                        column: x => x.team_id,
                        principalTable: "teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_team_members_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ticket_assigns",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ticket_id = table.Column<int>(type: "int", nullable: false),
                    team_id = table.Column<int>(type: "int", nullable: true),
                    team_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    user_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    assign_type = table.Column<string>(type: "nvarchar(5)", nullable: true),
                    viewed = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    viewed_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket_assign", x => x.id);
                    table.ForeignKey(
                        name: "FK_ticket_assigns_teams_team_id",
                        column: x => x.team_id,
                        principalTable: "teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ticket_assigns_tickets_ticket_id",
                        column: x => x.ticket_id,
                        principalTable: "tickets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ticket_assigns_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ticket_details",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ticket_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    comment = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    flag = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket_details", x => x.id);
                    table.ForeignKey(
                        name: "FK_ticket_details_tickets_ticket_id",
                        column: x => x.ticket_id,
                        principalTable: "tickets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ticket_details_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "medias",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    file_type = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    file_name = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    rel_id = table.Column<int>(type: "int", nullable: false),
                    rel_type = table.Column<string>(type: "nvarchar(5)", nullable: false),
                    ticket_id = table.Column<int>(type: "int", nullable: true),
                    ticket_detail_id = table.Column<int>(type: "int", nullable: true),
                    clog_id = table.Column<int>(type: "int", nullable: true),
                    kbase_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Media", x => x.id);
                    table.ForeignKey(
                        name: "FK_medias_clogs_clog_id",
                        column: x => x.clog_id,
                        principalTable: "clogs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_medias_kbases_kbase_id",
                        column: x => x.kbase_id,
                        principalTable: "kbases",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_medias_ticket_details_ticket_detail_id",
                        column: x => x.ticket_detail_id,
                        principalTable: "ticket_details",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_medias_tickets_ticket_id",
                        column: x => x.ticket_id,
                        principalTable: "tickets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "apps",
                columns: new[] { "id", "desc", "logo", "name" },
                values: new object[] { 1, "Integrated Advertising System", "Apps/Sysad.jpg", "SysAd" });

            migrationBuilder.InsertData(
                table: "departments",
                columns: new[] { "id", "desc", "name" },
                values: new object[,]
                {
                    { 4, "", "Other" },
                    { 3, "", "Programmer" },
                    { 2, "", "CS" },
                    { 1, "", "Management" }
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "desc", "name" },
                values: new object[,]
                {
                    { 4, "", "User" },
                    { 2, "", "Leader" },
                    { 1, "", "SuperAdmin" },
                    { 3, "", "Manager" }
                });

            migrationBuilder.InsertData(
                table: "senders",
                columns: new[] { "id", "color", "created_at", "email", "first_name", "image", "last_name", "login_status", "password", "salt", "updated_at" },
                values: new object[,]
                {
                    { 3, null, new DateTime(2021, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "cclienttiga@gmail.com", "CClient", "Users/cclienttiga.jpg", "Tiga", true, "FAB00430ABE5C0764C96D1F2A274D0C873F546F43876DA1DCE234614AF80D6CF59CF5629B0003BC7D19936F69773F431EA79C970DDB7A512C5DEC70FDC524592", "a38f49ef-23ef-424b-81ed-7e55cc32e512", null },
                    { 2, null, new DateTime(2021, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "bclientdua@gmail.com", "BClient", "Users/bclientdua.jpg", "Dua", null, "", "", null },
                    { 1, null, new DateTime(2021, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "aclientsatu@gmail.com", "AClient", "Users/aclientsatu.jpg", "Satu", null, "", "", null }
                });

            migrationBuilder.InsertData(
                table: "stats",
                columns: new[] { "id", "color", "desc", "name" },
                values: new object[,]
                {
                    { 6, "Grey", "bg-dark", "Reject" },
                    { 1, "Red", "bg-danger", "New" },
                    { 5, "Green", "bg-success", "Solve" },
                    { 2, "Sky", "bg-info", "Open" },
                    { 3, "Blue", "bg-primary", "In Progress" },
                    { 4, "Yellow", "bg-warning", "Pending" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "color", "created_at", "email", "first_name", "image", "last_name", "password", "salt", "updated_at" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2021, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "adminsuper@epsylonhome.com", "Admin", "Users/adminsuper.jpg", "Super", "407CEFA8AD88C93B16D48CB8303F0585C2F78B93679C6AD301C536DA16A9D1523E20E4AF705AF4EB5A843BB8076B60494B7665C11A18412265948CA475E584E9", "a38f49ef-23ef-424b-81ed-7e55cc32e512", null },
                    { 2, null, new DateTime(2021, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "teamleadcs@epsylonhome.com", "Leader", "Users/teamleadcs.jpg", "CS", "DDD0453584F966303A636EF96F1035DE28ECD38996ADEA93C10871BCF3DF17F494635C3325ED63080EA907664BB53F1CF3A1A0FBC688B8F87F5E950B85BC5D17", "a38f49ef-23ef-424b-81ed-7e55cc32e512", null },
                    { 3, null, new DateTime(2021, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "teamleaddev@epsylonhome.com", "Leader", "Users/teamleaddev.jpg", "DEV", "FBB9D3BFF4322EF2898162853C53A969250C11F81B8BFFBF6ED45CF00F2C56D1FC59D8C6C639243345C37CABBB40E14651DBD312239DC66432F3CBDC12E78FEE", "a38f49ef-23ef-424b-81ed-7e55cc32e512", null },
                    { 4, null, new DateTime(2021, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "managercs@epsylonhome.com", "Manager", "Users/managercs.jpg", "CS", "E6986E67760C1A620688ACDE9C70B820BC10F96D6F6E252B3E09911A5FFAD074E910CA1CABE6B172C6A7AF9C930D47F3A6AAC5C5F98A3C7523E06AF668ED0FE1", "a38f49ef-23ef-424b-81ed-7e55cc32e512", null },
                    { 5, null, new DateTime(2021, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "managerdev@epsylonhome.com", "Manager", "Users/managerdev.jpg", "DEV", "DC2CEF3CF54A7358E5F78F662047D2ED0ECD6DCE2BDF5750E8E17BDC570183ADB147C7DCB9E901EA8AA13D5E33BCBAF3328E26A9ECCC715A85D860AA90AEF182", "a38f49ef-23ef-424b-81ed-7e55cc32e512", null },
                    { 6, null, new DateTime(2021, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ausercs1@epsylonhome.com", "AUser", null, "CS1", "9D884826B8CB8D177EF68DFE5B72636C9553096B55818B4FCB0EAE5451EDEFAE449A0EE1A36ED6129EFB0342F6E62955836305E3FFE674DD66A894FC417218E7", "a38f49ef-23ef-424b-81ed-7e55cc32e512", null },
                    { 7, null, new DateTime(2021, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "buserdev1@epsylonhome.com", "BUser", null, "DEV1", "7E72CE48984272672CF610E5E6EC4118062B8CA4DF74BE35505919BBF6968DA9287024C5F22E2FCDF8BAC5C54D27C9D1A30DF5A17258866896D00809894A8BF4", "a38f49ef-23ef-424b-81ed-7e55cc32e512", null },
                    { 8, null, new DateTime(2021, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "cusercs2@epsylonhome.com", "CUser", null, "CS2", "EF7B9AA23F5EA170A0B95BBDDED4FAE5AEA6F2A45D905536B36EC0FFFD25E776921E364DC511C26CEA2567267E6CD237E3BB641A9BAEE9843CF45C88A7AF9E7A", "a38f49ef-23ef-424b-81ed-7e55cc32e512", null },
                    { 9, null, new DateTime(2021, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "duserdev2@epsylonhome.com", "DUser", null, "DEV2", "F8C8B2901079DDBE6FB140DBBB203E7A189BC6162386EBA269777A53C1833BA91256BB81AD735FEB0252945EE0013667F887D00CEA138139E4062A0847499DD4", "a38f49ef-23ef-424b-81ed-7e55cc32e512", null }
                });

            migrationBuilder.InsertData(
                table: "modules",
                columns: new[] { "id", "app_id", "desc", "name" },
                values: new object[,]
                {
                    { 1, 1, "", "Media" },
                    { 2, 1, "", "Production" },
                    { 3, 1, "", "Finance" },
                    { 4, 1, "", "Others" }
                });

            migrationBuilder.InsertData(
                table: "teams",
                columns: new[] { "id", "color", "created_by", "created_at", "desc", "image", "manager_id", "name", "updated_at" },
                values: new object[,]
                {
                    { 2, null, null, null, "", null, 5, "TEAM PROG1", null },
                    { 1, null, null, null, "", null, 4, "TEAM CS1", null }
                });

            migrationBuilder.InsertData(
                table: "user_depatments",
                columns: new[] { "id", "dept_id", "user_id" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 3, 2, 2 },
                    { 9, 2, 8 },
                    { 4, 3, 3 },
                    { 10, 3, 9 },
                    { 5, 2, 4 },
                    { 8, 3, 7 },
                    { 2, 3, 1 },
                    { 6, 3, 5 },
                    { 7, 2, 6 }
                });

            migrationBuilder.InsertData(
                table: "user_roles",
                columns: new[] { "id", "role_id", "user_id" },
                values: new object[,]
                {
                    { 9, 4, 8 },
                    { 8, 4, 7 },
                    { 7, 4, 6 },
                    { 1, 1, 1 },
                    { 5, 3, 4 },
                    { 4, 2, 3 },
                    { 3, 2, 2 },
                    { 2, 4, 1 },
                    { 6, 3, 5 },
                    { 10, 4, 9 }
                });

            migrationBuilder.InsertData(
                table: "team_members",
                columns: new[] { "id", "team_id", "user_id" },
                values: new object[,]
                {
                    { 1, 1, 6 },
                    { 2, 1, 8 },
                    { 3, 2, 7 },
                    { 4, 2, 9 }
                });

            migrationBuilder.CreateIndex(
                name: "idx_name",
                table: "apps",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "idx_name",
                table: "departments",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_medias_clog_id",
                table: "medias",
                column: "clog_id");

            migrationBuilder.CreateIndex(
                name: "IX_medias_kbase_id",
                table: "medias",
                column: "kbase_id");

            migrationBuilder.CreateIndex(
                name: "IX_medias_ticket_detail_id",
                table: "medias",
                column: "ticket_detail_id");

            migrationBuilder.CreateIndex(
                name: "IX_medias_ticket_id",
                table: "medias",
                column: "ticket_id");

            migrationBuilder.CreateIndex(
                name: "idx_name",
                table: "modules",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_modules_app_id",
                table: "modules",
                column: "app_id");

            migrationBuilder.CreateIndex(
                name: "idx_name",
                table: "roles",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "idx_sender",
                table: "senders",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "idx_name",
                table: "stats",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_team_members_team_id",
                table: "team_members",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "IX_team_members_user_id",
                table: "team_members",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_name",
                table: "teams",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_teams_manager_id",
                table: "teams",
                column: "manager_id");

            migrationBuilder.CreateIndex(
                name: "IX_ticket_assigns_team_id",
                table: "ticket_assigns",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "IX_ticket_assigns_ticket_id",
                table: "ticket_assigns",
                column: "ticket_id");

            migrationBuilder.CreateIndex(
                name: "IX_ticket_assigns_user_id",
                table: "ticket_assigns",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_ticket_details_ticket_id",
                table: "ticket_details",
                column: "ticket_id");

            migrationBuilder.CreateIndex(
                name: "IX_ticket_details_user_id",
                table: "ticket_details",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_TicketNumber",
                table: "tickets",
                column: "ticket_number");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_app_id",
                table: "tickets",
                column: "app_id");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_module_id",
                table: "tickets",
                column: "module_id");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_sender_id",
                table: "tickets",
                column: "sender_id");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_stat_id",
                table: "tickets",
                column: "stat_id");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_user_id",
                table: "tickets",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_userid",
                table: "user_depatments",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_depatments_dept_id",
                table: "user_depatments",
                column: "dept_id");

            migrationBuilder.CreateIndex(
                name: "idx_userid",
                table: "user_roles",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_roles_role_id",
                table: "user_roles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "idx_email",
                table: "users",
                column: "email");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "faqs");

            migrationBuilder.DropTable(
                name: "medias");

            migrationBuilder.DropTable(
                name: "team_members");

            migrationBuilder.DropTable(
                name: "ticket_assigns");

            migrationBuilder.DropTable(
                name: "user_depatments");

            migrationBuilder.DropTable(
                name: "user_roles");

            migrationBuilder.DropTable(
                name: "clogs");

            migrationBuilder.DropTable(
                name: "kbases");

            migrationBuilder.DropTable(
                name: "ticket_details");

            migrationBuilder.DropTable(
                name: "teams");

            migrationBuilder.DropTable(
                name: "departments");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "tickets");

            migrationBuilder.DropTable(
                name: "modules");

            migrationBuilder.DropTable(
                name: "senders");

            migrationBuilder.DropTable(
                name: "stats");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "apps");
        }
    }
}
