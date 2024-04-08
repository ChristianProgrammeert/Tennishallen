using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Tennishallen.Migrations
{
    /// <inheritdoc />
    public partial class Binbows : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Birthdate = table.Column<DateOnly>(type: "date", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    HourlyWage = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CoachId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourtId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourtId1 = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Hour = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_Courts_CourtId1",
                        column: x => x.CourtId1,
                        principalTable: "Courts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reservations_Users_CoachId",
                        column: x => x.CoachId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reservations_Users_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Active", "Address", "Birthdate", "City", "Email", "FirstName", "HourlyWage", "LastName", "Password", "Phone", "PostalCode" },
                values: new object[,]
                {
                    { new Guid("88605bf0-bdd3-45b7-8d7f-5fd8426212ff"), true, "Tennisstraat 1", new DateOnly(1, 1, 1), "Hengelo", "r.vandongen@tennishallenhengelo.nl", "Richard", 0.0, "van Dongen", "$2a$10$2IrWj2.r72iMrwkk4Bja/uWQbjaTV27OMSFIpfc1afX3t.eQLMZR2", "06 12345678", "1234TB" },
                    { new Guid("8bb581f0-f2f5-463a-8c66-1008ee947271"), true, "Tennisstraat 5", new DateOnly(1, 1, 1), "Hengelo", "C.Racket@tennishallenhengelo.nl", "Courtney", 0.0, "Racket", "$2a$10$bArkXdRVH7iLyz0894lNVOjv4UAHQgNhrINeaNSBGyYUQh83Ys8/G", "06 94685537", "1234TB" },
                    { new Guid("8e454215-eb40-493e-bd0f-c6c876016425"), true, "Tennisstraat 3", new DateOnly(1, 1, 1), "Hengelo", "G.Slam@tennishallenhengelo.nl", "Grant", 0.0, "Slam", "$2a$10$QJQUmIFsz1L/U9qiD64YAOpw6M4p9ay04.NKIczdSYSW/qy0R.7Du", "06 67316808", "1234TB" },
                    { new Guid("9decae06-d0ee-4081-ae4a-59648730ba4d"), true, "Tennisstraat 4", new DateOnly(1, 1, 1), "Hengelo", "T.Ishbahl@tennishallenhengelo.nl", "Tehn", 0.0, "Ishbahl", "$2a$10$WtOo1xjQPsLon5y3qC4Kh.bcaXlgM6xltGOvvxB6PUkm3nDKObOuO", "06 17884958", "1234TB" },
                    { new Guid("df7d2185-899d-481f-83db-cc9e1c97753c"), true, "Tennisstraat 2", new DateOnly(1, 1, 1), "Hengelo", "B.Hand@tennishallenhengelo.nl", "Beck", 0.0, "Hand", "$2a$10$ugEr.WK1ypE0bU6aHK1ISuT33VX7n43AMrjKWZV6dXgMG60Mp50dm", "06 55334567", "1234TB" }
                });

            migrationBuilder.InsertData(
                table: "Groups",
                columns: new[] { "Id", "Name", "UserId" },
                values: new object[,]
                {
                    { 1, 1, new Guid("88605bf0-bdd3-45b7-8d7f-5fd8426212ff") },
                    { 2, 0, new Guid("df7d2185-899d-481f-83db-cc9e1c97753c") },
                    { 3, 0, new Guid("8e454215-eb40-493e-bd0f-c6c876016425") },
                    { 4, 0, new Guid("9decae06-d0ee-4081-ae4a-59648730ba4d") },
                    { 5, 0, new Guid("8bb581f0-f2f5-463a-8c66-1008ee947271") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Groups_UserId",
                table: "Groups",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_CoachId",
                table: "Reservations",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_CourtId1",
                table: "Reservations",
                column: "CourtId1");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_MemberId",
                table: "Reservations",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Courts");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
