using Microsoft.EntityFrameworkCore.Migrations;

namespace Book.API.Migrations
{
    public partial class ean_isbn_fields_AreNowEntityAndImageIsNowValueObject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_Books_BookId",
                table: "Image");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Image",
                table: "Image");

            migrationBuilder.DropIndex(
                name: "IX_Image_BookId",
                table: "Image");

            migrationBuilder.DropIndex(
                name: "IX_Books_Ean13_Code",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_Isbn10_Code",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_Isbn13_Code",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Ean13_Code",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Isbn10_Code",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Isbn13_Code",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "_code",
                table: "Books");

            migrationBuilder.AlterColumn<int>(
                name: "BookId",
                table: "Image",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Isbn10Id",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Isbn13Id",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Image",
                table: "Image",
                columns: new[] { "BookId", "Id" });

            migrationBuilder.CreateTable(
                name: "BookEan13",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookEan13", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookIsbn10",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookIsbn10", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookIsbn13",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookIsbn13", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_Isbn10Id",
                table: "Books",
                column: "Isbn10Id");

            migrationBuilder.CreateIndex(
                name: "IX_Books_Isbn13Id",
                table: "Books",
                column: "Isbn13Id");

            migrationBuilder.CreateIndex(
                name: "IX_BookEan13_Code",
                table: "BookEan13",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BookIsbn10_Code",
                table: "BookIsbn10",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BookIsbn13_Code",
                table: "BookIsbn13",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_BookIsbn10_Isbn10Id",
                table: "Books",
                column: "Isbn10Id",
                principalTable: "BookIsbn10",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_BookIsbn13_Isbn13Id",
                table: "Books",
                column: "Isbn13Id",
                principalTable: "BookIsbn13",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Books_BookId",
                table: "Image",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_BookIsbn10_Isbn10Id",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_BookIsbn13_Isbn13Id",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Image_Books_BookId",
                table: "Image");

            migrationBuilder.DropTable(
                name: "BookEan13");

            migrationBuilder.DropTable(
                name: "BookIsbn10");

            migrationBuilder.DropTable(
                name: "BookIsbn13");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Image",
                table: "Image");

            migrationBuilder.DropIndex(
                name: "IX_Books_Isbn10Id",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_Isbn13Id",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Isbn10Id",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Isbn13Id",
                table: "Books");

            migrationBuilder.AlterColumn<int>(
                name: "BookId",
                table: "Image",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Ean13_Code",
                table: "Books",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Isbn10_Code",
                table: "Books",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Isbn13_Code",
                table: "Books",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "_code",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Image",
                table: "Image",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Image_BookId",
                table: "Image",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_Ean13_Code",
                table: "Books",
                column: "Ean13_Code",
                unique: true,
                filter: "[Ean13_Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Books_Isbn10_Code",
                table: "Books",
                column: "Isbn10_Code",
                unique: true,
                filter: "[Isbn10_Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Books_Isbn13_Code",
                table: "Books",
                column: "Isbn13_Code",
                unique: true,
                filter: "[Isbn13_Code] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Books_BookId",
                table: "Image",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
