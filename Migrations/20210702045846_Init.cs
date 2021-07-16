using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TicketingApi.Migrations
{
    public partial class Init : Migration
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
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true)
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
                    leader_id = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.id);
                    table.ForeignKey(
                        name: "FK_teams_users_leader_id",
                        column: x => x.leader_id,
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
                    supject = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    comment = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    app_id = table.Column<int>(type: "int", nullable: false),
                    module_id = table.Column<int>(type: "int", nullable: false),
                    sender_id = table.Column<int>(type: "int", nullable: false),
                    stat_id = table.Column<int>(type: "int", nullable: false),
                    solved_by = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    SolvedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    rejected_by = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    rejected_reason = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RejectedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tickets_stats_stat_id",
                        column: x => x.stat_id,
                        principalTable: "stats",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "medias",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    file_type = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    file_name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    rel_id = table.Column<int>(type: "int", nullable: false),
                    rel_type = table.Column<string>(type: "nvarchar(5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Media", x => x.id);
                    table.ForeignKey(
                        name: "FK_medias_tickets_rel_id",
                        column: x => x.rel_id,
                        principalTable: "tickets",
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
                    { 1, "", "Management" },
                    { 2, "", "CS" }
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "desc", "name" },
                values: new object[,]
                {
                    { 4, "", "User" },
                    { 3, "", "Leader" },
                    { 2, "", "Manager" },
                    { 1, "", "SuperAdmin" }
                });

            migrationBuilder.InsertData(
                table: "senders",
                columns: new[] { "id", "color", "created_at", "email", "first_name", "image", "last_name", "login_status", "password", "salt", "updated_at" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2021, 7, 5, 11, 14, 42, 355, DateTimeKind.Local).AddTicks(39), "daniel@epsylonhome.com", "Daniel", "Users/daniel.jpg", "Radcliff", null, "", "", null },
                    { 2, null, new DateTime(2021, 7, 5, 11, 14, 42, 355, DateTimeKind.Local).AddTicks(67), "ruppert@gmail.com", "Ruppert", "Users/ruppert.jpg", "Grint", null, "", "", null },
                    { 3, null, new DateTime(2021, 7, 5, 11, 14, 42, 355, DateTimeKind.Local).AddTicks(837), "emma@gmail.com", "Emma", "Users/emma.jpg", "Watson", true, "6F17907203B97583C3823F0F0DD6764EE0B886D5D613B8AE2D2056D13EC62A3014BCE11754FF58CD28D965BCF65AF722B2AB5CA21D5B04B6533B64E0DE06748E", "3f07720c-afb0-4228-8c07-d3ab11e9403a", null }
                });

            migrationBuilder.InsertData(
                table: "stats",
                columns: new[] { "id", "color", "desc", "name" },
                values: new object[,]
                {
                    { 4, "Blue", "", "Resolved" },
                    { 3, "Red", "", "In Progress" },
                    { 2, "Orange", "", "Open" },
                    { 1, "Green", "", "New" },
                    { 5, "Grey", "", "Rejected" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "color", "created_at", "email", "first_name", "image", "last_name", "password", "salt", "updated_at" },
                values: new object[,]
                {
                    { 6, null, new DateTime(2021, 7, 5, 11, 14, 42, 348, DateTimeKind.Local).AddTicks(8148), "vickyindiary@yahoo.com", "Scarlett", null, "Johansson", "0FADA9AB0700F00CC1A23FE47317F0F39B0ED2044BAE4FA016EFE4E9F49A9BA9F0D2EC8C18E73A080CFEA147F57DE6670B22A7B395C535BFDB9FBB029EF8C99B", "3f07720c-afb0-4228-8c07-d3ab11e9403a", null },
                    { 1, null, new DateTime(2021, 7, 5, 11, 14, 42, 348, DateTimeKind.Local).AddTicks(2294), "vicky.indiarto@epsylonhome.com", "Vicky", "Users/vicky.jpg", "Epsylon", "DC9E3434996F7BDCBFD82E501FCB1FFD3DEBDD633DB1D48159860CCF704EF06C8340575D8AA4BA496113AD57BE39CF4780FD8008B7FE2863AC35F741FDB57075", "3f07720c-afb0-4228-8c07-d3ab11e9403a", null },
                    { 2, null, new DateTime(2021, 7, 5, 11, 14, 42, 348, DateTimeKind.Local).AddTicks(5232), "vickyindiarto@gmail.com", "Crish", "Users/crish.jpg", "Evans", "3ECC9B45B3BAB8874506903B0AF10E396404EA626EC1E4660E547937358E2867A0507E7CC3224978EB134091BAFBC9E5CCD8DB7976E0AC34B80BA55CEA0D1A57", "3f07720c-afb0-4228-8c07-d3ab11e9403a", null },
                    { 3, null, new DateTime(2021, 7, 5, 11, 14, 42, 348, DateTimeKind.Local).AddTicks(5990), "vickynewonline@gmail.com", "Mark", "Users/mark.jpg", "Ruffalo", "3FFAB749BA5F97C4D4C6A50BB619F2787F83C37EC8AF15F8266DA1D6AF5BB886586AB50ACBD1458756A813917D11809BEA8045E3490FDDAC019A0EF3F6CF3B89", "3f07720c-afb0-4228-8c07-d3ab11e9403a", null },
                    { 4, null, new DateTime(2021, 7, 5, 11, 14, 42, 348, DateTimeKind.Local).AddTicks(6743), "vickyindiar@yahoo.com", "Robert", "Users/robert.jpg", "Downy", "73FF221CB27E67158721C25C1EBCF385D50B602664ADCE3A4F9C992A88DFC1952A06271619A2345E4E6933B9A3758E78FB16AF3F131A07AB0BF179B7E1474237", "3f07720c-afb0-4228-8c07-d3ab11e9403a", null },
                    { 5, null, new DateTime(2021, 7, 5, 11, 14, 42, 348, DateTimeKind.Local).AddTicks(7417), "vickyindiarx@yahoo.com", "Tom", "Users/tom.jpg", "Holan", "D1397C183EC569F14A02508E35319904EBAF5CAB06E5E25FF10B6FA385E48373FA60DD6B10412499AF6CB2DCEC0FF1C68D0A03779D36CCE97ECF64ABCF3CB50C", "3f07720c-afb0-4228-8c07-d3ab11e9403a", null },
                    { 7, null, new DateTime(2021, 7, 5, 11, 14, 42, 349, DateTimeKind.Local).AddTicks(594), "vickyindiarz@yahoo.com", "Jeremy", null, "Renner", "1DE1BB972E38417E377F4CA56DFE899C2D8957C9B7CD5045D3D1E79908ACA8A2F7AD2218EA9090C5D680F51F50F0C8ACD551255C7AD187A9FB7467C15F479434", "3f07720c-afb0-4228-8c07-d3ab11e9403a", null }
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
                columns: new[] { "id", "CreatedAt", "desc", "leader_id", "name", "UpdatedAt" },
                values: new object[] { 1, null, "", 4, "TEAM CAP", null });

            migrationBuilder.InsertData(
                table: "user_depatments",
                columns: new[] { "id", "dept_id", "user_id" },
                values: new object[,]
                {
                    { 7, 2, 6 },
                    { 6, 3, 5 },
                    { 5, 2, 4 },
                    { 8, 2, 7 },
                    { 4, 3, 3 },
                    { 3, 2, 2 },
                    { 2, 3, 1 },
                    { 1, 1, 1 }
                });

            migrationBuilder.InsertData(
                table: "user_roles",
                columns: new[] { "id", "role_id", "user_id" },
                values: new object[,]
                {
                    { 4, 2, 3 },
                    { 2, 4, 1 },
                    { 1, 1, 1 },
                    { 5, 3, 4 },
                    { 6, 3, 5 },
                    { 7, 2, 6 },
                    { 3, 2, 2 },
                    { 8, 2, 7 }
                });

            migrationBuilder.InsertData(
                table: "team_members",
                columns: new[] { "id", "team_id", "user_id" },
                values: new object[,]
                {
                    { 1, 1, 6 },
                    { 2, 1, 7 }
                });

            migrationBuilder.InsertData(
                table: "tickets",
                columns: new[] { "id", "app_id", "comment", "created_at", "module_id", "RejectedAt", "rejected_by", "rejected_reason", "sender_id", "SolvedAt", "solved_by", "stat_id", "supject", "ticket_number", "updated_at" },
                values: new object[,]
                {
                    { 1, 1, "lorem ipsu sdkskadn ksdnksin jdnskjdna jsandjkansdjkansd jndsajkdnajkd kasjndsndoqm dolor shit nyoasdasdaslibay knoper low", new DateTime(2021, 7, 5, 11, 14, 42, 359, DateTimeKind.Local).AddTicks(3206), 1, null, null, null, 1, null, null, 3, "Ini Test Subject satu ", "180620211", new DateTime(2021, 7, 5, 11, 14, 42, 359, DateTimeKind.Local).AddTicks(3224) },
                    { 2, 1, "asdhjkahsdjas jasdjj sjadnajk jasnd jas d asndjka  skjdnaksjdn sshdjkajksdas jashdjkahsjkd oashdasihsjskaslnsk", new DateTime(2021, 7, 5, 11, 14, 42, 359, DateTimeKind.Local).AddTicks(5109), 1, null, null, null, 2, null, null, 1, "Subject for ticket number 2", "180620212", new DateTime(2021, 7, 5, 11, 14, 42, 359, DateTimeKind.Local).AddTicks(5118) },
                    { 3, 1, "ksknnina  lasklk  klsnklna ksaiopoellss ksdoasjdandanwdwqo sdnskandjasd  jskdnjksanda asndndiqwioqdwq", new DateTime(2021, 7, 5, 11, 14, 42, 359, DateTimeKind.Local).AddTicks(5121), 1, null, null, null, 3, null, null, 1, "Subjecsdskkks ksnkandkasndk t 3", "180620213", new DateTime(2021, 7, 5, 11, 14, 42, 359, DateTimeKind.Local).AddTicks(5123) }
                });

            migrationBuilder.InsertData(
                table: "medias",
                columns: new[] { "id", "file_name", "file_type", "rel_id", "rel_type" },
                values: new object[,]
                {
                    { 1, "Tickets/atc1.jpg", "image", 1, "T" },
                    { 2, "Tickets/atc2.pdf", "pdf", 1, "T" },
                    { 3, "TicketDetails/atc3.jpeg", "image", 1, "TD" },
                    { 4, "TicketsDetails/atc4.xls", "excel", 1, "TD" }
                });

            migrationBuilder.InsertData(
                table: "ticket_assigns",
                columns: new[] { "id", "assign_type", "team_at", "team_id", "ticket_id", "user_at", "user_id", "viewed", "viewed_at" },
                values: new object[,]
                {
                    { 1, "M", null, null, 1, new DateTime(2021, 7, 5, 11, 14, 42, 369, DateTimeKind.Local).AddTicks(8933), 2, true, new DateTime(2021, 7, 5, 11, 14, 42, 369, DateTimeKind.Local).AddTicks(8946) },
                    { 2, "T", new DateTime(2021, 7, 5, 11, 14, 42, 370, DateTimeKind.Local).AddTicks(592), 1, 1, null, 3, true, new DateTime(2021, 7, 5, 11, 14, 42, 370, DateTimeKind.Local).AddTicks(601) },
                    { 3, "U", null, null, 1, new DateTime(2021, 7, 5, 11, 14, 42, 370, DateTimeKind.Local).AddTicks(2240), 4, true, new DateTime(2021, 7, 5, 11, 14, 42, 370, DateTimeKind.Local).AddTicks(2251) }
                });

            migrationBuilder.InsertData(
                table: "ticket_details",
                columns: new[] { "id", "comment", "created_at", "ticket_id", "updated_at", "user_id" },
                values: new object[,]
                {
                    { 1, "lorem ipsum dolor shit nyolibay kksdj nknop ksiola knoper low", new DateTime(2021, 7, 5, 11, 14, 42, 368, DateTimeKind.Local).AddTicks(6594), 1, new DateTime(2021, 7, 5, 11, 14, 42, 368, DateTimeKind.Local).AddTicks(6620), 4 },
                    { 2, "asdhjkahsdjas jasshdjkajksdas jashdjkahsjkd oashdasihsjskaslnsk", new DateTime(2021, 7, 5, 11, 14, 42, 368, DateTimeKind.Local).AddTicks(8224), 1, new DateTime(2021, 7, 5, 11, 14, 42, 368, DateTimeKind.Local).AddTicks(8234), null },
                    { 3, "ksknnina  lasklk  klsnklna ksaiopoells;mlauw klnskoiskel aksnkadia mkaskks ", new DateTime(2021, 7, 5, 11, 14, 42, 368, DateTimeKind.Local).AddTicks(9477), 1, new DateTime(2021, 7, 5, 11, 14, 42, 368, DateTimeKind.Local).AddTicks(9486), 4 }
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
                name: "IX_medias_rel_id",
                table: "medias",
                column: "rel_id");

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
                name: "IX_teams_leader_id",
                table: "teams",
                column: "leader_id");

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
                name: "ticket_details");

            migrationBuilder.DropTable(
                name: "user_depatments");

            migrationBuilder.DropTable(
                name: "user_roles");

            migrationBuilder.DropTable(
                name: "teams");

            migrationBuilder.DropTable(
                name: "tickets");

            migrationBuilder.DropTable(
                name: "departments");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "modules");

            migrationBuilder.DropTable(
                name: "senders");

            migrationBuilder.DropTable(
                name: "stats");

            migrationBuilder.DropTable(
                name: "apps");
        }
    }
}
