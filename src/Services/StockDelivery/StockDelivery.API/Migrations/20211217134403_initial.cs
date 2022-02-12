using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StockDelivery.API.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActiveDeliveries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsAllDeliveryItemsScanned = table.Column<bool>(type: "bit", nullable: false),
                    IsAnyDeliveryItemsScanned = table.Column<bool>(type: "bit", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveDeliveries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookStocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookEan13_Code = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: true),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    _code = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookStocks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CancelledDeliveries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ActiveDeliveryId = table.Column<int>(type: "int", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CancellationReason = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CancelledDeliveries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompletedDeliveries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ActiveDeliveryId = table.Column<int>(type: "int", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletedDeliveries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActiveDeliveryItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActiveDeliveryId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    BookEan_Code = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: true),
                    ItemsCount = table.Column<short>(type: "smallint", nullable: false),
                    ScannedCount = table.Column<short>(type: "smallint", nullable: false),
                    IsScanned = table.Column<bool>(type: "bit", nullable: false),
                    IsAllScanned = table.Column<bool>(type: "bit", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    _code = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveDeliveryItem", x => new { x.ActiveDeliveryId, x.Id });
                    table.ForeignKey(
                        name: "FK_ActiveDeliveryItem_ActiveDeliveries_ActiveDeliveryId",
                        column: x => x.ActiveDeliveryId,
                        principalTable: "ActiveDeliveries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CancelledDeliveryItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CancelledDeliveryId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    BookEan_Code = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: true),
                    ItemsCount = table.Column<short>(type: "smallint", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    _code = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CancelledDeliveryItem", x => new { x.CancelledDeliveryId, x.Id });
                    table.ForeignKey(
                        name: "FK_CancelledDeliveryItem_CancelledDeliveries_CancelledDeliveryId",
                        column: x => x.CancelledDeliveryId,
                        principalTable: "CancelledDeliveries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompletedDeliveryItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompletedDeliveryId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    BookEan_Code = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: true),
                    ItemsCount = table.Column<short>(type: "smallint", nullable: false),
                    Stocks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    _code = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletedDeliveryItem", x => new { x.CompletedDeliveryId, x.Id });
                    table.ForeignKey(
                        name: "FK_CompletedDeliveryItem_CompletedDeliveries_CompletedDeliveryId",
                        column: x => x.CompletedDeliveryId,
                        principalTable: "CompletedDeliveries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActiveDeliveryItem_BookEan_Code",
                table: "ActiveDeliveryItem",
                column: "BookEan_Code",
                unique: true,
                filter: "[BookEan_Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CancelledDeliveryItem_BookEan_Code",
                table: "CancelledDeliveryItem",
                column: "BookEan_Code",
                unique: true,
                filter: "[BookEan_Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CompletedDeliveryItem_BookEan_Code",
                table: "CompletedDeliveryItem",
                column: "BookEan_Code",
                unique: true,
                filter: "[BookEan_Code] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActiveDeliveryItem");

            migrationBuilder.DropTable(
                name: "BookStocks");

            migrationBuilder.DropTable(
                name: "CancelledDeliveryItem");

            migrationBuilder.DropTable(
                name: "CompletedDeliveryItem");

            migrationBuilder.DropTable(
                name: "ActiveDeliveries");

            migrationBuilder.DropTable(
                name: "CancelledDeliveries");

            migrationBuilder.DropTable(
                name: "CompletedDeliveries");
        }
    }
}
