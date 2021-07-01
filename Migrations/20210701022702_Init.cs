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
                    image = table.Column<string>(type: "nvarchar(50)", nullable: true),
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
                    { 2, "", "CS" },
                    { 1, "", "Management" },
                    { 3, "", "Programmer" }
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "desc", "name" },
                values: new object[,]
                {
                    { 1, "", "SuperAdmin" },
                    { 2, "", "Manager" },
                    { 3, "", "Leader" },
                    { 4, "", "User" }
                });

            migrationBuilder.InsertData(
                table: "senders",
                columns: new[] { "id", "created_at", "email", "first_name", "image", "last_name", "login_status", "password", "salt", "updated_at" },
                values: new object[,]
                {
                    { 1, new DateTime(2021, 7, 1, 9, 27, 0, 783, DateTimeKind.Local).AddTicks(9600), "daniel@epsylonhome.com", "Daniel", "Users/daniel.jpg", "Radcliff", null, "", "", null },
                    { 2, new DateTime(2021, 7, 1, 9, 27, 0, 783, DateTimeKind.Local).AddTicks(9629), "ruppert@gmail.com", "Ruppert", "Users/ruppert.jpg", "Grint", null, "", "", null },
                    { 3, new DateTime(2021, 7, 1, 9, 27, 0, 784, DateTimeKind.Local).AddTicks(663), "emma@gmail.com", "Emma", "Users/emma.jpg", "Watson", true, "4C71BE4019263CCCA9BEE7DAAC7D034A88DC6CB717CDB800A92456EABFC4C6C35600265FC79AC508D91079F9D7BCFFC0247BE4D3F4C74CC89066C41C47B40BD2", "cb15fd53-e640-4dfe-a7ae-fc11f83a914e", null }
                });

            migrationBuilder.InsertData(
                table: "stats",
                columns: new[] { "id", "color", "desc", "name" },
                values: new object[,]
                {
                    { 5, "Grey", "", "Rejected" },
                    { 3, "Red", "", "In Progress" },
                    { 2, "Orange", "", "Open" },
                    { 1, "Green", "", "New" },
                    { 4, "Blue", "", "Resolved" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "created_at", "email", "first_name", "image", "last_name", "password", "salt", "updated_at" },
                values: new object[,]
                {
                    { 3, new DateTime(2021, 7, 1, 9, 27, 0, 776, DateTimeKind.Local).AddTicks(1068), "vickynewonline@gmail.com", "Mark", "Users/mark.jpg", "Ruffalo", "8ED8D655F859A6780399C0A47339E4903D020C6EC1E71A26276C65DC64BB1907AFA2F6CE8F108A9041023AC71302A670E660CB26877A90CD896B8E1BE5359BF5", "cb15fd53-e640-4dfe-a7ae-fc11f83a914e", null },
                    { 1, new DateTime(2021, 7, 1, 9, 27, 0, 775, DateTimeKind.Local).AddTicks(7808), "vicky.indiarto@epsylonhome.com", "Vicky", "Users/vicky.jpg", "Epsylon", "5EDEAB632CA45F5F5E3082931953A958F269F267BE51951336BAD167A8A1186E6132C0E04DF8F51DB30EEAD15488CEB873BE68905C0762FCEDF436539FA48F28", "cb15fd53-e640-4dfe-a7ae-fc11f83a914e", null },
                    { 2, new DateTime(2021, 7, 1, 9, 27, 0, 776, DateTimeKind.Local).AddTicks(552), "vickyindiarto@gmail.com", "Crish", "Users/crish.jpg", "Evans", "1ADCC8B4C84A76FA422FFD0E05AAA759B2E3098752219681BD96104AFC1649DC4F5A8CF6085E6E35E8024231350271433B6C07240EEB36296ABC6CAB34DFBD5C", "cb15fd53-e640-4dfe-a7ae-fc11f83a914e", null },
                    { 4, new DateTime(2021, 7, 1, 9, 27, 0, 776, DateTimeKind.Local).AddTicks(1660), "vickyindiar@yahoo.com", "RobertDowny", "Users/robert.jpg", "Downy", "21500C76171723B6B292C2D7CA4FBE5027C9A31839962E0BA75A0E2BB4C9A7D6EAB70B78C327709227AD9FF45DFC8AFAFDBCEC44C0563BD825B147765DC6ECEE", "cb15fd53-e640-4dfe-a7ae-fc11f83a914e", null }
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
                values: new object[] { 1, null, "", 3, "TEAM CAP", null });

            migrationBuilder.InsertData(
                table: "user_depatments",
                columns: new[] { "id", "dept_id", "user_id" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 3, 1 },
                    { 3, 2, 2 },
                    { 4, 2, 3 },
                    { 5, 2, 4 }
                });

            migrationBuilder.InsertData(
                table: "user_roles",
                columns: new[] { "id", "role_id", "user_id" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 4, 1 },
                    { 3, 2, 2 },
                    { 4, 3, 3 },
                    { 5, 2, 4 }
                });

            migrationBuilder.InsertData(
                table: "team_members",
                columns: new[] { "id", "team_id", "user_id" },
                values: new object[,]
                {
                    { 1, 1, 4 },
                    { 2, 1, 3 }
                });

            migrationBuilder.InsertData(
                table: "tickets",
                columns: new[] { "id", "app_id", "comment", "created_at", "module_id", "RejectedAt", "rejected_by", "rejected_reason", "sender_id", "SolvedAt", "solved_by", "stat_id", "supject", "ticket_number", "updated_at" },
                values: new object[,]
                {
                    { 1, 1, "lorem ipsu sdkskadn ksdnksin jdnskjdna jsandjkansdjkansd jndsajkdnajkd kasjndsndoqm dolor shit nyoasdasdaslibay knoper low", new DateTime(2021, 7, 1, 9, 27, 0, 788, DateTimeKind.Local).AddTicks(7413), 1, null, null, null, 1, null, null, 3, "Ini Test Subject satu ", "180620211", new DateTime(2021, 7, 1, 9, 27, 0, 788, DateTimeKind.Local).AddTicks(7432) },
                    { 2, 1, "asdhjkahsdjas jasdjj sjadnajk jasnd jas d asndjka  skjdnaksjdn sshdjkajksdas jashdjkahsjkd oashdasihsjskaslnsk", new DateTime(2021, 7, 1, 9, 27, 0, 788, DateTimeKind.Local).AddTicks(9401), 1, null, null, null, 2, null, null, 1, "Subject for ticket number 2", "180620212", new DateTime(2021, 7, 1, 9, 27, 0, 788, DateTimeKind.Local).AddTicks(9410) },
                    { 3, 1, "ksknnina  lasklk  klsnklna ksaiopoellss ksdoasjdandanwdwqo sdnskandjasd  jskdnjksanda asndndiqwioqdwq", new DateTime(2021, 7, 1, 9, 27, 0, 788, DateTimeKind.Local).AddTicks(9414), 1, null, null, null, 3, null, null, 1, "Subjecsdskkks ksnkandkasndk t 3", "180620213", new DateTime(2021, 7, 1, 9, 27, 0, 788, DateTimeKind.Local).AddTicks(9416) }
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
                    { 1, "M", null, null, 1, new DateTime(2021, 7, 1, 9, 27, 0, 800, DateTimeKind.Local).AddTicks(6081), 2, true, new DateTime(2021, 7, 1, 9, 27, 0, 800, DateTimeKind.Local).AddTicks(6093) },
                    { 2, "T", new DateTime(2021, 7, 1, 9, 27, 0, 800, DateTimeKind.Local).AddTicks(7718), 1, 1, null, 3, true, new DateTime(2021, 7, 1, 9, 27, 0, 800, DateTimeKind.Local).AddTicks(7726) },
                    { 3, "U", null, null, 1, new DateTime(2021, 7, 1, 9, 27, 0, 800, DateTimeKind.Local).AddTicks(9161), 4, true, new DateTime(2021, 7, 1, 9, 27, 0, 800, DateTimeKind.Local).AddTicks(9169) }
                });

            migrationBuilder.InsertData(
                table: "ticket_details",
                columns: new[] { "id", "comment", "created_at", "ticket_id", "updated_at", "user_id" },
                values: new object[,]
                {
                    { 1, "lorem ipsum dolor shit nyolibay kksdj nknop ksiola knoper low", new DateTime(2021, 7, 1, 9, 27, 0, 799, DateTimeKind.Local).AddTicks(3150), 1, new DateTime(2021, 7, 1, 9, 27, 0, 799, DateTimeKind.Local).AddTicks(3177), 4 },
                    { 2, "asdhjkahsdjas jasshdjkajksdas jashdjkahsjkd oashdasihsjskaslnsk", new DateTime(2021, 7, 1, 9, 27, 0, 799, DateTimeKind.Local).AddTicks(5241), 1, new DateTime(2021, 7, 1, 9, 27, 0, 799, DateTimeKind.Local).AddTicks(5250), null },
                    { 3, "ksknnina  lasklk  klsnklna ksaiopoells;mlauw klnskoiskel aksnkadia mkaskks ", new DateTime(2021, 7, 1, 9, 27, 0, 799, DateTimeKind.Local).AddTicks(6793), 1, new DateTime(2021, 7, 1, 9, 27, 0, 799, DateTimeKind.Local).AddTicks(6802), 4 }
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
