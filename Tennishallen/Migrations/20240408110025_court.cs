using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Tennishallen.Migrations
{
    /// <inheritdoc />
    public partial class court : Migration
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
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CoachId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CourtId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Hour = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_Courts_CourtId",
                        column: x => x.CourtId,
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
                    { new Guid("2db2301d-4e06-49f1-82be-3f9ddbc64da8"), true, "Tennisstraat 1", new DateOnly(1, 1, 1), "Hengelo", "r.vandongen@tennishallenhengelo.nl", "Richard", 0.0, "van Dongen", "$2a$10$0dvXFxGlhIbMQF0Za7tCx.TypVAiDATGO7jh4ohcXaL77oHWV/qHW", "06 12345678", "1234TB" },
                    { new Guid("2dd43519-a8e0-4831-a646-732026386805"), true, "Tennisstraat 2", new DateOnly(1, 1, 1), "Hengelo", "B.Hand@tennishallenhengelo.nl", "Beck", 0.0, "Hand", "$2a$10$XqZLSpyR9Gr3.jH/J1zt5OEKVvOJxeD5SP7geZQxyts49sIzhsmNS", "06 98152523", "1234TB" },
                    { new Guid("b1ee2eb9-1ff5-4e8c-b542-b1e2138887b6"), true, "Tennisstraat 5", new DateOnly(1, 1, 1), "Hengelo", "C.Racket@tennishallenhengelo.nl", "Courtney", 0.0, "Racket", "$2a$10$JXGHodl1IAn5pQkBq5cCLeHZg8vwYBunYqyelJ5igi4.tGC5JUTIi", "06 25018196", "1234TB" },
                    { new Guid("f663d196-ac33-431d-9a55-3c53f554ebb6"), true, "Tennisstraat 3", new DateOnly(1, 1, 1), "Hengelo", "G.Slam@tennishallenhengelo.nl", "Grant", 0.0, "Slam", "$2a$10$KQCndWH.YX9NKtRj.Q.vIu251ETtVQLp7M/WpugdSk3xD45ZKQrwS", "06 64665037", "1234TB" },
                    { new Guid("f722fe00-bfcc-4814-88c3-896c39a6113d"), true, "Tennisstraat 4", new DateOnly(1, 1, 1), "Hengelo", "T.Ishbahl@tennishallenhengelo.nl", "Tehn", 0.0, "Ishbahl", "$2a$10$0SlB7lC4GrEjB5MCDrP0DuVBaBRw..RTtNvGNJiopVi/yiRsQn2QO", "06 50917581", "1234TB" }
                });

            migrationBuilder.InsertData(
                table: "Groups",
                columns: new[] { "Id", "Name", "UserId" },
                values: new object[,]
                {
                    { 1, 0, new Guid("2db2301d-4e06-49f1-82be-3f9ddbc64da8") },
                    { 2, 0, new Guid("2dd43519-a8e0-4831-a646-732026386805") },
                    { 3, 0, new Guid("f663d196-ac33-431d-9a55-3c53f554ebb6") },
                    { 4, 0, new Guid("f722fe00-bfcc-4814-88c3-896c39a6113d") },
                    { 5, 0, new Guid("b1ee2eb9-1ff5-4e8c-b542-b1e2138887b6") }
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
                name: "IX_Reservations_CourtId",
                table: "Reservations",
                column: "CourtId");

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
