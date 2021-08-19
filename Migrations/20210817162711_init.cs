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
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
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
                    TicketsId = table.Column<int>(type: "int", nullable: true),
                    TicketDetailsId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Media", x => x.id);
                    table.ForeignKey(
                        name: "FK_medias_ticket_details_TicketDetailsId",
                        column: x => x.TicketDetailsId,
                        principalTable: "ticket_details",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_medias_tickets_TicketsId",
                        column: x => x.TicketsId,
                        principalTable: "tickets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "apps",
                columns: new[] { "id", "desc", "logo", "name" },
                values: new object[,]
                {
                    { 1, "Integrated Advertising System", "Apps/Sysad.jpg", "SysAd" },
                    { 2, "", null, "App2" },
                    { 3, "", null, "App3" },
                    { 4, "", null, "APP4" }
                });

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
                    { 3, "", "Manager" },
                    { 2, "", "Leader" },
                    { 1, "", "SuperAdmin" }
                });

            migrationBuilder.InsertData(
                table: "senders",
                columns: new[] { "id", "color", "created_at", "email", "first_name", "image", "last_name", "login_status", "password", "salt", "updated_at" },
                values: new object[,]
                {
                    { 3, null, new DateTime(2021, 8, 17, 23, 27, 10, 192, DateTimeKind.Local).AddTicks(5549), "cclienttiga@gmail.com", "CClient", "Users/cclienttiga.jpg", "Tiga", true, "7A3E8A48F1AFF97ACBF233B356C5A56EB423AF2FBCC1EE6831B0F4DF1E7F48556017A43E7EBF325F6E9CF8A85AA95905DC581CFF395C1C4EA829A35EEBDC91FA", "07d81016-8b37-42ff-8f0a-489ebde74047", null },
                    { 2, null, new DateTime(2021, 8, 17, 23, 27, 10, 192, DateTimeKind.Local).AddTicks(4789), "bclientdua@gmail.com", "BClient", "Users/bclientdua.jpg", "Dua", null, "", "", null },
                    { 1, null, new DateTime(2021, 8, 17, 23, 27, 10, 192, DateTimeKind.Local).AddTicks(4769), "aclientsatu@gmail.com", "AClient", "Users/aclientsatu.jpg", "Satu", null, "", "", null }
                });

            migrationBuilder.InsertData(
                table: "stats",
                columns: new[] { "id", "color", "desc", "name" },
                values: new object[,]
                {
                    { 6, "Grey", "bg-dark", "Reject" },
                    { 1, "Red", "bg-danger", "New" },
                    { 5, "Green", "bg-success", "Solve" },
                    { 3, "Blue", "bg-primary", "In Progress" },
                    { 4, "Yellow", "bg-warning", "Pending" },
                    { 2, "Sky", "bg-info", "Open" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "color", "created_at", "email", "first_name", "image", "last_name", "password", "salt", "updated_at" },
                values: new object[,]
                {
                    { 8, null, new DateTime(2021, 8, 17, 23, 27, 10, 186, DateTimeKind.Local).AddTicks(9827), "cusercs2@epsylonhome.com", "CUser", null, "CS2", "383B1C92EA059ACDD13F33F985400F159C2B5C44E45149ACB9145417EBC6ED6708F752AE88B03DEA7A9232B65E72310442947062028D08D7914DF0890D6363B5", "07d81016-8b37-42ff-8f0a-489ebde74047", null },
                    { 1, null, new DateTime(2021, 8, 17, 23, 27, 10, 186, DateTimeKind.Local).AddTicks(2110), "adminsuper@epsylonhome.com", "Admin", "Users/adminsuper.jpg", "Super", "E43368C058D8D3222B52EB4CF83A2FD894036AFF6D67FBF964D2D17499BEA35DAEC34D7F1C44050FFB6B34D0B41701E2B402BFCE78AE08EAEF94BB92A67C8817", "07d81016-8b37-42ff-8f0a-489ebde74047", null },
                    { 2, null, new DateTime(2021, 8, 17, 23, 27, 10, 186, DateTimeKind.Local).AddTicks(4848), "teamleadcs@epsylonhome.com", "Leader", "Users/teamleadcs.jpg", "CS", "F8E909F9EB746B56B56399E040B903C13E2577629D7A57FF9BEFE4C087A6EBAC661D48D91F7DD315FE282E4FC6D2EB4AD9726879053EE5893DA32D197A7F8C89", "07d81016-8b37-42ff-8f0a-489ebde74047", null },
                    { 3, null, new DateTime(2021, 8, 17, 23, 27, 10, 186, DateTimeKind.Local).AddTicks(5369), "teamleaddev@epsylonhome.com", "Leader", "Users/teamleaddev.jpg", "DEV", "434C1F12CDA9C087754983FC7BAFBA0BF9928EDA3CE1A23A7111068A698E694196E2703A826A7A18523A8979001DC8AF056BDF0C2D4EEADF358F922A1A50BF5A", "07d81016-8b37-42ff-8f0a-489ebde74047", null },
                    { 4, null, new DateTime(2021, 8, 17, 23, 27, 10, 186, DateTimeKind.Local).AddTicks(5907), "managercs@epsylonhome.com", "Manager", "Users/managercs.jpg", "CS", "7F72F5ECB5974C08B13A9931639021CB1A8487FCAF358AFCDC0EA3A728734796C90DF5E58CA4B5F5BA62DA29D75658451C0107FBE5850C1FFCE6C15E855CC021", "07d81016-8b37-42ff-8f0a-489ebde74047", null },
                    { 5, null, new DateTime(2021, 8, 17, 23, 27, 10, 186, DateTimeKind.Local).AddTicks(6414), "managerdev@epsylonhome.com", "Manager", "Users/managerdev.jpg", "DEV", "EDBBD8F51B34ECA2FF307AD7722D2397F8DAACB474170546A5B004B1AD23F0981954851CBFB7700CB9A7AE80A18A5DF4AE7D13BB61CBF21BD70583D662F3BD8B", "07d81016-8b37-42ff-8f0a-489ebde74047", null },
                    { 6, null, new DateTime(2021, 8, 17, 23, 27, 10, 186, DateTimeKind.Local).AddTicks(6984), "ausercs1@epsylonhome.com", "AUser", null, "CS1", "BC821F48E9DCC8E96EFCFCCD7D16078A48EDB84A381A10D42A1EBDA567DB733A827B559698CBE1EF23C013EBE497D08CA111034CD0CED5D08B4C5EE1329216AC", "07d81016-8b37-42ff-8f0a-489ebde74047", null },
                    { 7, null, new DateTime(2021, 8, 17, 23, 27, 10, 186, DateTimeKind.Local).AddTicks(9313), "buserdev1@epsylonhome.com", "BUser", null, "DEV1", "22EC875439A30004E61FEC45F99760D462B009EEC6BD2F340A160918F89D38947C7C9563EE685DB78AC835DC71D624666D3C5A5A068B22DABE9DA65DA16BEB15", "07d81016-8b37-42ff-8f0a-489ebde74047", null },
                    { 9, null, new DateTime(2021, 8, 17, 23, 27, 10, 187, DateTimeKind.Local).AddTicks(334), "duserdev2@epsylonhome.com", "DUser", null, "DEV2", "3C078BF7812F18247F5879F8EDCDEE56394B5BAE5E8936A612A5E2DFB221F6FD3C063A6E8C5CA0F77BBC05E3B9B51DF6D232F04D5D2BAEC689CD27FFFEFC328F", "07d81016-8b37-42ff-8f0a-489ebde74047", null }
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
                columns: new[] { "id", "CreatedAt", "desc", "manager_id", "name", "UpdatedAt" },
                values: new object[,]
                {
                    { 2, null, "", 5, "TEAM PROG1", null },
                    { 1, null, "", 4, "TEAM CS1", null }
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
                name: "IX_medias_TicketDetailsId",
                table: "medias",
                column: "TicketDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_medias_TicketsId",
                table: "medias",
                column: "TicketsId");

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
                name: "kbases");

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
