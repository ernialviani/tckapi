using Microsoft.EntityFrameworkCore.Migrations;

namespace TicketingApi.Migrations
{
    public partial class add_c_private_t_ticket_detail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "private",
                table: "ticket_details",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "private",
                table: "ticket_details");
        }
    }
}
