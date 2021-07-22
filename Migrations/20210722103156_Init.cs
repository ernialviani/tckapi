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
                    solved_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    rejected_by = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    rejected_reason = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    rejected_at = table.Column<DateTime>(type: "datetime", nullable: true),
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
                    { 1, "", "Management" },
                    { 2, "", "CS" }
                });

            migrationBuilder.InsertData(
                table: "medias",
                columns: new[] { "id", "file_name", "file_type", "rel_id", "rel_type", "TicketDetailsId", "TicketsId" },
                values: new object[,]
                {
                    { 3, "TicketDetails/atc3.jpeg", ".jpg", 1, "TD", null, null },
                    { 4, "TicketsDetails/atc4.xls", ".xls", 1, "TD", null, null },
                    { 2, "Tickets/atc2.pdf", ".pdf", 1, "T", null, null },
                    { 1, "Tickets/atc1.jpg", ".pf", 1, "T", null, null }
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "desc", "name" },
                values: new object[,]
                {
                    { 2, "", "Manager" },
                    { 4, "", "User" },
                    { 3, "", "Leader" },
                    { 1, "", "SuperAdmin" }
                });

            migrationBuilder.InsertData(
                table: "senders",
                columns: new[] { "id", "color", "created_at", "email", "first_name", "image", "last_name", "login_status", "password", "salt", "updated_at" },
                values: new object[,]
                {
                    { 3, null, new DateTime(2021, 7, 22, 17, 31, 55, 372, DateTimeKind.Local).AddTicks(9227), "emma@gmail.com", "Emma", "Users/emma.jpg", "Watson", true, "D4DBF83B073AC6E9BDC94B6B872657DD47D23B1DC42BC72D00B06EA76E3B3B45C85B213C33C0096B82E0954E30D3C2178E7F8FEF5037700AE8E945CDC9018DBA", "3f532883-bcc5-40d5-8f42-5fdf5e241a51", null },
                    { 2, null, new DateTime(2021, 7, 22, 17, 31, 55, 372, DateTimeKind.Local).AddTicks(8522), "ruppert@gmail.com", "Ruppert", "Users/ruppert.jpg", "Grint", null, "", "", null },
                    { 1, null, new DateTime(2021, 7, 22, 17, 31, 55, 372, DateTimeKind.Local).AddTicks(8490), "daniel@epsylonhome.com", "Daniel", "Users/daniel.jpg", "Radcliff", null, "", "", null }
                });

            migrationBuilder.InsertData(
                table: "stats",
                columns: new[] { "id", "color", "desc", "name" },
                values: new object[,]
                {
                    { 1, "Green", "", "New" },
                    { 6, "Grey", "", "Reject" },
                    { 5, "Blue", "", "Solve" },
                    { 4, "Yellow", "", "Pending" },
                    { 3, "Red", "", "In Progress" },
                    { 2, "Orange", "", "Open" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "color", "created_at", "email", "first_name", "image", "last_name", "password", "salt", "updated_at" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2021, 7, 22, 17, 31, 55, 366, DateTimeKind.Local).AddTicks(6455), "vicky.indiarto@epsylonhome.com", "Vicky", "Users/vicky.jpg", "Epsylon", "10FD0607D57BC447945DAF5B0F5E7DA9EAFE144E3F853447DE443951FDE6DE46E1C6DF29B9FAEA0E4E2BC605A153A8AAEAC49AF81D73B391481EF9688206F47F", "3f532883-bcc5-40d5-8f42-5fdf5e241a51", null },
                    { 2, null, new DateTime(2021, 7, 22, 17, 31, 55, 366, DateTimeKind.Local).AddTicks(9302), "vickyindiarto@gmail.com", "Crish", "Users/crish.jpg", "Evans", "BCAFF0132B92027CB20845D7B8F7E02A66A1B11FA0E4E0A8ECC114D66DC196D37AEBE290EA32433F0F7A412248786AB85A27CBA9419465A14DC60FDD30613CBE", "3f532883-bcc5-40d5-8f42-5fdf5e241a51", null },
                    { 3, null, new DateTime(2021, 7, 22, 17, 31, 55, 366, DateTimeKind.Local).AddTicks(9835), "vickynewonline@gmail.com", "Mark", "Users/mark.jpg", "Ruffalo", "52C36130A9F843D088724E5EBDB0737CFA343969BA50ADEC2A37E3EEBE369D2C9CDCBC09F6EAA7AE74A061870FF9605D6289C58C2FC7679AB072A813876D1A05", "3f532883-bcc5-40d5-8f42-5fdf5e241a51", null },
                    { 4, null, new DateTime(2021, 7, 22, 17, 31, 55, 367, DateTimeKind.Local).AddTicks(365), "vickyindiar@yahoo.com", "Robert", "Users/robert.jpg", "Downy", "6280CC9C7617836A54E71415E03A6D8EE21724524F2F3A152EF80C29A207F95AFA8D0CA7CDF73817552A544A7156B6FAA144F3C58B77A37B1E03BC2CFD51D95E", "3f532883-bcc5-40d5-8f42-5fdf5e241a51", null },
                    { 5, null, new DateTime(2021, 7, 22, 17, 31, 55, 367, DateTimeKind.Local).AddTicks(863), "vickyindiarx@yahoo.com", "Tom", "Users/tom.jpg", "Holan", "8F17F616697882E5451FF270DCC4660015E259B88F9EF25D127FE350DA2976F8758C30126FD10EC3D4C9DCFEA4CFDB60B178B2606E0FB13F42390ECDE481CF90", "3f532883-bcc5-40d5-8f42-5fdf5e241a51", null },
                    { 6, null, new DateTime(2021, 7, 22, 17, 31, 55, 367, DateTimeKind.Local).AddTicks(1370), "vickyindiary@yahoo.com", "Scarlett", null, "Johansson", "D8D86394D2F363E8C122D416ABD2E2124E82CA1AE3911E5A4E7A8B853C84A27CC628CAC0E6E1D22E3EA4AA1F380FC0857DD1CA0D9531BE08CAB733C704756C84", "3f532883-bcc5-40d5-8f42-5fdf5e241a51", null },
                    { 7, null, new DateTime(2021, 7, 22, 17, 31, 55, 367, DateTimeKind.Local).AddTicks(3617), "vickyindiarz@yahoo.com", "Jeremy", null, "Renner", "3756816E5B79280EC30A3B615C2862EB91AA53A2587B86B681002A3A84359307A7C20D327FFF46FCBC369AC6C64852CD144F6A7DEC6AA8719CE6750581940443", "3f532883-bcc5-40d5-8f42-5fdf5e241a51", null }
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
                    { 7, 4, 6 },
                    { 3, 2, 2 },
                    { 8, 4, 7 }
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
                columns: new[] { "id", "app_id", "comment", "created_at", "module_id", "rejected_at", "rejected_by", "rejected_reason", "sender_id", "solved_at", "solved_by", "stat_id", "supject", "ticket_number", "updated_at" },
                values: new object[,]
                {
                    { 1, 1, "lorem ipsu sdkskadn ksdnksin jdnskjdna jsandjkansdjkansd jndsajkdnajkd kasjndsndoqm dolor shit nyoasdasdaslibay knoper low", new DateTime(2021, 7, 22, 17, 31, 55, 377, DateTimeKind.Local).AddTicks(2714), 1, null, null, null, 1, null, null, 3, "Ini Test Subject satu ", "180620211", new DateTime(2021, 7, 22, 17, 31, 55, 377, DateTimeKind.Local).AddTicks(2731) },
                    { 2, 1, "asdhjkahsdjas jasdjj sjadnajk jasnd jas d asndjka  skjdnaksjdn sshdjkajksdas jashdjkahsjkd oashdasihsjskaslnsk", new DateTime(2021, 7, 22, 17, 31, 55, 377, DateTimeKind.Local).AddTicks(4920), 1, null, null, null, 2, null, null, 1, "Subject for ticket number 2", "180620212", new DateTime(2021, 7, 22, 17, 31, 55, 377, DateTimeKind.Local).AddTicks(4929) },
                    { 3, 1, "ksknnina  lasklk  klsnklna ksaiopoellss ksdoasjdandanwdwqo sdnskandjasd  jskdnjksanda asndndiqwioqdwq", new DateTime(2021, 7, 22, 17, 31, 55, 377, DateTimeKind.Local).AddTicks(4932), 1, null, null, null, 3, null, null, 1, "Subjecsdskkks ksnkandkasndk t 3", "180620213", new DateTime(2021, 7, 22, 17, 31, 55, 377, DateTimeKind.Local).AddTicks(4934) }
                });

            migrationBuilder.InsertData(
                table: "ticket_assigns",
                columns: new[] { "id", "assign_type", "team_at", "team_id", "ticket_id", "user_at", "user_id", "viewed", "viewed_at" },
                values: new object[,]
                {
                    { 1, "M", null, null, 1, new DateTime(2021, 7, 22, 17, 31, 55, 387, DateTimeKind.Local).AddTicks(516), 2, true, new DateTime(2021, 7, 22, 17, 31, 55, 387, DateTimeKind.Local).AddTicks(527) },
                    { 2, "T", new DateTime(2021, 7, 22, 17, 31, 55, 387, DateTimeKind.Local).AddTicks(2164), 1, 1, null, 3, true, new DateTime(2021, 7, 22, 17, 31, 55, 387, DateTimeKind.Local).AddTicks(2174) },
                    { 3, "U", null, null, 1, new DateTime(2021, 7, 22, 17, 31, 55, 387, DateTimeKind.Local).AddTicks(3650), 4, true, new DateTime(2021, 7, 22, 17, 31, 55, 387, DateTimeKind.Local).AddTicks(3657) }
                });

            migrationBuilder.InsertData(
                table: "ticket_details",
                columns: new[] { "id", "comment", "created_at", "ticket_id", "updated_at", "user_id" },
                values: new object[,]
                {
                    { 1, "lorem ipsum dolor shit nyolibay kksdj nknop ksiola knoper low", new DateTime(2021, 7, 22, 17, 31, 55, 385, DateTimeKind.Local).AddTicks(9096), 1, new DateTime(2021, 7, 22, 17, 31, 55, 385, DateTimeKind.Local).AddTicks(9117), 4 },
                    { 2, "asdhjkahsdjas jasshdjkajksdas jashdjkahsjkd oashdasihsjskaslnsk", new DateTime(2021, 7, 22, 17, 31, 55, 386, DateTimeKind.Local).AddTicks(720), 1, new DateTime(2021, 7, 22, 17, 31, 55, 386, DateTimeKind.Local).AddTicks(731), null },
                    { 3, "ksknnina  lasklk  klsnklna ksaiopoells;mlauw klnskoiskel aksnkadia mkaskks ", new DateTime(2021, 7, 22, 17, 31, 55, 386, DateTimeKind.Local).AddTicks(1977), 1, new DateTime(2021, 7, 22, 17, 31, 55, 386, DateTimeKind.Local).AddTicks(1986), 4 }
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
