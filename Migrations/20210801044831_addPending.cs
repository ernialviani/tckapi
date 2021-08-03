using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TicketingApi.Migrations
{
    public partial class addPending : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_medias_ticket_details_TicketDetailsId",
                table: "medias");

            migrationBuilder.DropForeignKey(
                name: "FK_medias_tickets_TicketsId",
                table: "medias");

            migrationBuilder.DropIndex(
                name: "IX_medias_TicketDetailsId",
                table: "medias");

            migrationBuilder.DropIndex(
                name: "IX_medias_TicketsId",
                table: "medias");

            migrationBuilder.DropColumn(
                name: "TicketDetailsId",
                table: "medias");

            migrationBuilder.DropColumn(
                name: "TicketsId",
                table: "medias");

            migrationBuilder.AddColumn<DateTime>(
                name: "pending_at",
                table: "tickets",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "pending_by",
                table: "tickets",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "senders",
                keyColumn: "id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2021, 8, 1, 11, 48, 30, 168, DateTimeKind.Local).AddTicks(2503));

            migrationBuilder.UpdateData(
                table: "senders",
                keyColumn: "id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2021, 8, 1, 11, 48, 30, 168, DateTimeKind.Local).AddTicks(2526));

            migrationBuilder.UpdateData(
                table: "senders",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "password", "salt" },
                values: new object[] { new DateTime(2021, 8, 1, 11, 48, 30, 168, DateTimeKind.Local).AddTicks(3257), "23C98B147B407C2F28CB6289652B694FB7962C05484773343C5B6AE2D7530173ED02F66432AF5906E7DCBDC9AB598BAA78617CBC94A86C98722F8BE92CC67DCB", "7c2b88d5-e079-4215-983b-1de3359f006c" });

            migrationBuilder.UpdateData(
                table: "ticket_assigns",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "user_at", "viewed_at" },
                values: new object[] { new DateTime(2021, 8, 1, 11, 48, 30, 183, DateTimeKind.Local).AddTicks(7967), new DateTime(2021, 8, 1, 11, 48, 30, 183, DateTimeKind.Local).AddTicks(7981) });

            migrationBuilder.UpdateData(
                table: "ticket_assigns",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "team_at", "viewed_at" },
                values: new object[] { new DateTime(2021, 8, 1, 11, 48, 30, 183, DateTimeKind.Local).AddTicks(9595), new DateTime(2021, 8, 1, 11, 48, 30, 183, DateTimeKind.Local).AddTicks(9603) });

            migrationBuilder.UpdateData(
                table: "ticket_assigns",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "user_at", "viewed_at" },
                values: new object[] { new DateTime(2021, 8, 1, 11, 48, 30, 184, DateTimeKind.Local).AddTicks(1041), new DateTime(2021, 8, 1, 11, 48, 30, 184, DateTimeKind.Local).AddTicks(1048) });

            migrationBuilder.UpdateData(
                table: "ticket_assigns",
                keyColumn: "id",
                keyValue: 4,
                column: "user_at",
                value: new DateTime(2021, 8, 1, 11, 48, 30, 184, DateTimeKind.Local).AddTicks(1052));

            migrationBuilder.UpdateData(
                table: "ticket_assigns",
                keyColumn: "id",
                keyValue: 5,
                column: "user_at",
                value: new DateTime(2021, 8, 1, 11, 48, 30, 184, DateTimeKind.Local).AddTicks(2235));

            migrationBuilder.UpdateData(
                table: "ticket_assigns",
                keyColumn: "id",
                keyValue: 6,
                column: "user_at",
                value: new DateTime(2021, 8, 1, 11, 48, 30, 184, DateTimeKind.Local).AddTicks(2243));

            migrationBuilder.UpdateData(
                table: "ticket_details",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2021, 8, 1, 11, 48, 30, 182, DateTimeKind.Local).AddTicks(5523), new DateTime(2021, 8, 1, 11, 48, 30, 182, DateTimeKind.Local).AddTicks(5546) });

            migrationBuilder.UpdateData(
                table: "ticket_details",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2021, 8, 1, 11, 48, 30, 182, DateTimeKind.Local).AddTicks(7132), new DateTime(2021, 8, 1, 11, 48, 30, 182, DateTimeKind.Local).AddTicks(7141) });

            migrationBuilder.UpdateData(
                table: "ticket_details",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2021, 8, 1, 11, 48, 30, 182, DateTimeKind.Local).AddTicks(8353), new DateTime(2021, 8, 1, 11, 48, 30, 182, DateTimeKind.Local).AddTicks(8361) });

            migrationBuilder.UpdateData(
                table: "tickets",
                keyColumn: "id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2021, 8, 1, 11, 48, 30, 174, DateTimeKind.Local).AddTicks(2221));

            migrationBuilder.UpdateData(
                table: "tickets",
                keyColumn: "id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2021, 8, 1, 11, 48, 30, 174, DateTimeKind.Local).AddTicks(4390));

            migrationBuilder.UpdateData(
                table: "tickets",
                keyColumn: "id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2021, 8, 1, 11, 48, 30, 174, DateTimeKind.Local).AddTicks(4401));

            migrationBuilder.UpdateData(
                table: "tickets",
                keyColumn: "id",
                keyValue: 4,
                column: "created_at",
                value: new DateTime(2021, 8, 1, 11, 48, 30, 174, DateTimeKind.Local).AddTicks(4403));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "password", "salt" },
                values: new object[] { new DateTime(2021, 8, 1, 11, 48, 30, 161, DateTimeKind.Local).AddTicks(8459), "7B4FA0B9A27740A662FEF3E4629BE9D11179C7805827618C472E0F17247E1754945283CC6027487B6684054E1A2BC552A6CE7F1DE9D7E720570D7F2B6187AEE5", "7c2b88d5-e079-4215-983b-1de3359f006c" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "password", "salt" },
                values: new object[] { new DateTime(2021, 8, 1, 11, 48, 30, 162, DateTimeKind.Local).AddTicks(1280), "16887D91A29BC85D4AC6C440365A43619CE85F809EE47B51F189A2891E7AF62CE03BFF2516AF77C0390334622B06594DC7AC2D0C582DAB923C76FD90207FCDCB", "7c2b88d5-e079-4215-983b-1de3359f006c" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "password", "salt" },
                values: new object[] { new DateTime(2021, 8, 1, 11, 48, 30, 162, DateTimeKind.Local).AddTicks(1812), "BB9E13B4BE8EAD79FBDC45AA04277C04CC69378D661E5BF44322EAE3D80054BB69CA471DDBCC3AA158E38BA735430DAF7F7FA2388D34392E20734E8B0CAAE3E8", "7c2b88d5-e079-4215-983b-1de3359f006c" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "created_at", "password", "salt" },
                values: new object[] { new DateTime(2021, 8, 1, 11, 48, 30, 162, DateTimeKind.Local).AddTicks(2382), "58B392EA0B2206849E8BD502653ED96A9D6D82D86C1373D3AE9A4AE8FA03EB991E95F2C39CA15274FCD7301CA8724B455867139D18A9F1A34E9EEC8A17F60DF4", "7c2b88d5-e079-4215-983b-1de3359f006c" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "created_at", "password", "salt" },
                values: new object[] { new DateTime(2021, 8, 1, 11, 48, 30, 162, DateTimeKind.Local).AddTicks(2898), "87DB3A7C0E48A4A31A74A57232128E8DC73EF195FA7EDC3886AC1BB23C956CF1A7C2574C799AAF7E2B04C10053CE1039E6F3BBDEB3F9B48EB5780FF7D066F52D", "7c2b88d5-e079-4215-983b-1de3359f006c" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "created_at", "password", "salt" },
                values: new object[] { new DateTime(2021, 8, 1, 11, 48, 30, 162, DateTimeKind.Local).AddTicks(3408), "0EC8D612F0DCCA1445E0DD67595AA85C4D8DACC9E51F9765D3F193D2392CD0F45294C47821FF6117C40560D5CD2FEF0BAA2CE9533A92C97F77FB44CDEB664197", "7c2b88d5-e079-4215-983b-1de3359f006c" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "created_at", "password", "salt" },
                values: new object[] { new DateTime(2021, 8, 1, 11, 48, 30, 162, DateTimeKind.Local).AddTicks(5710), "7DD204391258844B199DE3D263DF74F63155195C64376302E8CE600F10197815DC2792C221ADCB87607AEB826882F05B21452671971427C01F7F2F95C0F4E3C3", "7c2b88d5-e079-4215-983b-1de3359f006c" });

            migrationBuilder.CreateIndex(
                name: "IX_medias_rel_id",
                table: "medias",
                column: "rel_id");

            migrationBuilder.AddForeignKey(
                name: "FK_medias_ticket_details_rel_id",
                table: "medias",
                column: "rel_id",
                principalTable: "ticket_details",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_medias_tickets_rel_id",
                table: "medias",
                column: "rel_id",
                principalTable: "tickets",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_medias_ticket_details_rel_id",
                table: "medias");

            migrationBuilder.DropForeignKey(
                name: "FK_medias_tickets_rel_id",
                table: "medias");

            migrationBuilder.DropIndex(
                name: "IX_medias_rel_id",
                table: "medias");

            migrationBuilder.DropColumn(
                name: "pending_at",
                table: "tickets");

            migrationBuilder.DropColumn(
                name: "pending_by",
                table: "tickets");

            migrationBuilder.AddColumn<int>(
                name: "TicketDetailsId",
                table: "medias",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TicketsId",
                table: "medias",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "senders",
                keyColumn: "id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2021, 7, 27, 10, 39, 38, 646, DateTimeKind.Local).AddTicks(1708));

            migrationBuilder.UpdateData(
                table: "senders",
                keyColumn: "id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2021, 7, 27, 10, 39, 38, 646, DateTimeKind.Local).AddTicks(1732));

            migrationBuilder.UpdateData(
                table: "senders",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "password", "salt" },
                values: new object[] { new DateTime(2021, 7, 27, 10, 39, 38, 646, DateTimeKind.Local).AddTicks(2673), "64FEFC19C079C63DF213EE78C342DEA22EA24BF68E33A9D5B179A4C6493285235C8E26C2E2081B22161DC3BFCC536D15BC58CB7A8936FFCA103E3F24A0B71438", "01079031-d15e-4902-a79e-3f4b33963fb1" });

            migrationBuilder.UpdateData(
                table: "ticket_assigns",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "user_at", "viewed_at" },
                values: new object[] { new DateTime(2021, 7, 27, 10, 39, 38, 661, DateTimeKind.Local).AddTicks(2522), new DateTime(2021, 7, 27, 10, 39, 38, 661, DateTimeKind.Local).AddTicks(2535) });

            migrationBuilder.UpdateData(
                table: "ticket_assigns",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "team_at", "viewed_at" },
                values: new object[] { new DateTime(2021, 7, 27, 10, 39, 38, 661, DateTimeKind.Local).AddTicks(4151), new DateTime(2021, 7, 27, 10, 39, 38, 661, DateTimeKind.Local).AddTicks(4159) });

            migrationBuilder.UpdateData(
                table: "ticket_assigns",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "user_at", "viewed_at" },
                values: new object[] { new DateTime(2021, 7, 27, 10, 39, 38, 661, DateTimeKind.Local).AddTicks(5632), new DateTime(2021, 7, 27, 10, 39, 38, 661, DateTimeKind.Local).AddTicks(5639) });

            migrationBuilder.UpdateData(
                table: "ticket_assigns",
                keyColumn: "id",
                keyValue: 4,
                column: "user_at",
                value: new DateTime(2021, 7, 27, 10, 39, 38, 661, DateTimeKind.Local).AddTicks(5643));

            migrationBuilder.UpdateData(
                table: "ticket_assigns",
                keyColumn: "id",
                keyValue: 5,
                column: "user_at",
                value: new DateTime(2021, 7, 27, 10, 39, 38, 661, DateTimeKind.Local).AddTicks(6863));

            migrationBuilder.UpdateData(
                table: "ticket_assigns",
                keyColumn: "id",
                keyValue: 6,
                column: "user_at",
                value: new DateTime(2021, 7, 27, 10, 39, 38, 661, DateTimeKind.Local).AddTicks(6871));

            migrationBuilder.UpdateData(
                table: "ticket_details",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2021, 7, 27, 10, 39, 38, 660, DateTimeKind.Local).AddTicks(751), new DateTime(2021, 7, 27, 10, 39, 38, 660, DateTimeKind.Local).AddTicks(772) });

            migrationBuilder.UpdateData(
                table: "ticket_details",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2021, 7, 27, 10, 39, 38, 660, DateTimeKind.Local).AddTicks(2642), new DateTime(2021, 7, 27, 10, 39, 38, 660, DateTimeKind.Local).AddTicks(2651) });

            migrationBuilder.UpdateData(
                table: "ticket_details",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2021, 7, 27, 10, 39, 38, 660, DateTimeKind.Local).AddTicks(3888), new DateTime(2021, 7, 27, 10, 39, 38, 660, DateTimeKind.Local).AddTicks(3896) });

            migrationBuilder.UpdateData(
                table: "tickets",
                keyColumn: "id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2021, 7, 27, 10, 39, 38, 651, DateTimeKind.Local).AddTicks(6776));

            migrationBuilder.UpdateData(
                table: "tickets",
                keyColumn: "id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2021, 7, 27, 10, 39, 38, 651, DateTimeKind.Local).AddTicks(8816));

            migrationBuilder.UpdateData(
                table: "tickets",
                keyColumn: "id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2021, 7, 27, 10, 39, 38, 651, DateTimeKind.Local).AddTicks(8827));

            migrationBuilder.UpdateData(
                table: "tickets",
                keyColumn: "id",
                keyValue: 4,
                column: "created_at",
                value: new DateTime(2021, 7, 27, 10, 39, 38, 651, DateTimeKind.Local).AddTicks(8830));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "password", "salt" },
                values: new object[] { new DateTime(2021, 7, 27, 10, 39, 38, 639, DateTimeKind.Local).AddTicks(6188), "41F0070B1CAD67A1852B5F6F0E7D4A8F57ED768975C0D691B49AFCFC033F6A23A497EC68138C20573BDC162DC87086EC08748DEB18D87154B2AB94B92F1E7446", "01079031-d15e-4902-a79e-3f4b33963fb1" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "password", "salt" },
                values: new object[] { new DateTime(2021, 7, 27, 10, 39, 38, 639, DateTimeKind.Local).AddTicks(9064), "540A358BA70A8B2026E9737FC4DE40A4C9C48FA70C91BFC1503A09F428453128725CB10A811ACE5316F4432ED6F74BFF4C01707AD14E5FE3E766D4A522D0B40E", "01079031-d15e-4902-a79e-3f4b33963fb1" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "password", "salt" },
                values: new object[] { new DateTime(2021, 7, 27, 10, 39, 38, 639, DateTimeKind.Local).AddTicks(9840), "053877C492D096F9B5C227EF0D9315E93CE01877EBB2FA0EF9E53E71CDAEAD2531122B5212E0A3A4136CCF94B405B7D4E67EB95292EF63AF6BC77D6030703E7B", "01079031-d15e-4902-a79e-3f4b33963fb1" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "created_at", "password", "salt" },
                values: new object[] { new DateTime(2021, 7, 27, 10, 39, 38, 640, DateTimeKind.Local).AddTicks(577), "307D668DCA51F69CCDCDB6F0A400CB77559F6E5BA4F93BC0D0FF91921F7B16A9B5802A38F1AB6CCA246FD6B559CD702EA37C23F5A4FDD96A4D7B1416C438C0F3", "01079031-d15e-4902-a79e-3f4b33963fb1" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "created_at", "password", "salt" },
                values: new object[] { new DateTime(2021, 7, 27, 10, 39, 38, 640, DateTimeKind.Local).AddTicks(1309), "7B18DDDFDD9044A7F8E1164D81C9B4E80894FA3041AAF17BE546ACC9948D8210D7B2146257B57C10A0F3EABFACEAEA09C62EA867920DCBA59C2D221ACEE91687", "01079031-d15e-4902-a79e-3f4b33963fb1" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "created_at", "password", "salt" },
                values: new object[] { new DateTime(2021, 7, 27, 10, 39, 38, 640, DateTimeKind.Local).AddTicks(2038), "7C03E544F57C93339151EB81939899B0AFB6C39A38173FAB1279181BED1F841747AFE2CE057EDDD5A2723775C6D5FAD626AA57FE728283B24C5EEA9C0FB8E637", "01079031-d15e-4902-a79e-3f4b33963fb1" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "created_at", "password", "salt" },
                values: new object[] { new DateTime(2021, 7, 27, 10, 39, 38, 640, DateTimeKind.Local).AddTicks(4428), "CDC3FD1C80512180A16E4BDB581A05451286A5A4007B50544FE1EAD4392AB830CB2FCAA04BF7156CB96BADAFE2ABFE824C1C9CCF8788366EC57D7FD7513504D2", "01079031-d15e-4902-a79e-3f4b33963fb1" });

            migrationBuilder.CreateIndex(
                name: "IX_medias_TicketDetailsId",
                table: "medias",
                column: "TicketDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_medias_TicketsId",
                table: "medias",
                column: "TicketsId");

            migrationBuilder.AddForeignKey(
                name: "FK_medias_ticket_details_TicketDetailsId",
                table: "medias",
                column: "TicketDetailsId",
                principalTable: "ticket_details",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_medias_tickets_TicketsId",
                table: "medias",
                column: "TicketsId",
                principalTable: "tickets",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
