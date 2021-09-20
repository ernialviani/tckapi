using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TicketingApi.Migrations
{
    public partial class enhance_clog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_medias_clogs_clog_id",
                table: "medias");

            migrationBuilder.DropColumn(
                name: "module_id",
                table: "clogs");

            migrationBuilder.RenameColumn(
                name: "clog_id",
                table: "medias",
                newName: "clog_detail_id");

            migrationBuilder.RenameIndex(
                name: "IX_medias_clog_id",
                table: "medias",
                newName: "IX_medias_clog_detail_id");

            migrationBuilder.AlterColumn<string>(
                name: "version",
                table: "clogs",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)");

            migrationBuilder.AlterColumn<string>(
                name: "desc",
                table: "clogs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "clog_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    color = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    desc = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CLog_Type", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "clog_details",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    desc = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    clog_id = table.Column<int>(type: "int", nullable: false),
                    clog_type_id = table.Column<int>(type: "int", nullable: false),
                    module_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CLog_Detail", x => x.id);
                    table.ForeignKey(
                        name: "FK_clog_details_clog_type_clog_type_id",
                        column: x => x.clog_type_id,
                        principalTable: "clog_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_clog_details_clogs_clog_id",
                        column: x => x.clog_id,
                        principalTable: "clogs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_clog_details_modules_module_id",
                        column: x => x.module_id,
                        principalTable: "modules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "clog_type",
                columns: new[] { "id", "color", "desc", "name" },
                values: new object[] { 1, "Blue", null, "New" });

            migrationBuilder.InsertData(
                table: "clog_type",
                columns: new[] { "id", "color", "desc", "name" },
                values: new object[] { 2, "Orange", null, "Fix" });

            migrationBuilder.InsertData(
                table: "clog_type",
                columns: new[] { "id", "color", "desc", "name" },
                values: new object[] { 3, "Green", null, "Enhance" });

            migrationBuilder.CreateIndex(
                name: "IX_clogs_app_id",
                table: "clogs",
                column: "app_id");

            migrationBuilder.CreateIndex(
                name: "IX_clogs_user_id",
                table: "clogs",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_clog_details_clog_id",
                table: "clog_details",
                column: "clog_id");

            migrationBuilder.CreateIndex(
                name: "IX_clog_details_clog_type_id",
                table: "clog_details",
                column: "clog_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_clog_details_module_id",
                table: "clog_details",
                column: "module_id");

            migrationBuilder.AddForeignKey(
                name: "FK_clogs_apps_app_id",
                table: "clogs",
                column: "app_id",
                principalTable: "apps",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_clogs_users_user_id",
                table: "clogs",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_medias_clog_details_clog_detail_id",
                table: "medias",
                column: "clog_detail_id",
                principalTable: "clog_details",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_clogs_apps_app_id",
                table: "clogs");

            migrationBuilder.DropForeignKey(
                name: "FK_clogs_users_user_id",
                table: "clogs");

            migrationBuilder.DropForeignKey(
                name: "FK_medias_clog_details_clog_detail_id",
                table: "medias");

            migrationBuilder.DropTable(
                name: "clog_details");

            migrationBuilder.DropTable(
                name: "clog_type");

            migrationBuilder.DropIndex(
                name: "IX_clogs_app_id",
                table: "clogs");

            migrationBuilder.DropIndex(
                name: "IX_clogs_user_id",
                table: "clogs");

            migrationBuilder.RenameColumn(
                name: "clog_detail_id",
                table: "medias",
                newName: "clog_id");

            migrationBuilder.RenameIndex(
                name: "IX_medias_clog_detail_id",
                table: "medias",
                newName: "IX_medias_clog_id");

            migrationBuilder.AlterColumn<string>(
                name: "version",
                table: "clogs",
                type: "nvarchar(20)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");

            migrationBuilder.AlterColumn<string>(
                name: "desc",
                table: "clogs",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "module_id",
                table: "clogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_medias_clogs_clog_id",
                table: "medias",
                column: "clog_id",
                principalTable: "clogs",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
