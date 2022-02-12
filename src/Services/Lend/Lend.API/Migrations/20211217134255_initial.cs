using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lend.API.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LendedBaskets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LendedBaskets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SimpleBooleanRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RuleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RuleValue = table.Column<bool>(type: "bit", nullable: false),
                    RuleValueType = table.Column<int>(type: "int", nullable: false),
                    StrategyType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimpleBooleanRules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SimpleIntRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RuleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RuleValue = table.Column<int>(type: "int", nullable: false),
                    RuleValueType = table.Column<int>(type: "int", nullable: false),
                    StrategyType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimpleIntRules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LendedStock",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockId = table.Column<int>(type: "int", nullable: false),
                    BookEan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LendedBasketId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LendedStock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LendedStock_LendedBaskets_LendedBasketId",
                        column: x => x.LendedBasketId,
                        principalTable: "LendedBaskets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LendedBaskets_Email",
                table: "LendedBaskets",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_LendedStock_LendedBasketId",
                table: "LendedStock",
                column: "LendedBasketId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LendedStock");

            migrationBuilder.DropTable(
                name: "SimpleBooleanRules");

            migrationBuilder.DropTable(
                name: "SimpleIntRules");

            migrationBuilder.DropTable(
                name: "LendedBaskets");
        }
    }
}
