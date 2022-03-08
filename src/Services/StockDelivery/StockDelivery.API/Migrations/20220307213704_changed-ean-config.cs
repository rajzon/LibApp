using Microsoft.EntityFrameworkCore.Migrations;

namespace StockDelivery.API.Migrations
{
    public partial class changedeanconfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CompletedDeliveryItem_BookEan_Code",
                table: "CompletedDeliveryItem");

            migrationBuilder.CreateIndex(
                name: "IX_CompletedDeliveryItem_BookEan_Code",
                table: "CompletedDeliveryItem",
                column: "BookEan_Code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CompletedDeliveryItem_BookEan_Code",
                table: "CompletedDeliveryItem");

            migrationBuilder.CreateIndex(
                name: "IX_CompletedDeliveryItem_BookEan_Code",
                table: "CompletedDeliveryItem",
                column: "BookEan_Code",
                unique: true,
                filter: "[BookEan_Code] IS NOT NULL");
        }
    }
}
