using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace User.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Adres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostCode_Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Post = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AddressCorrespondence",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Adres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostCode_Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Post = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressCorrespondence", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Email",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Email", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdCard",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdCard", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailId = table.Column<int>(type: "int", nullable: true),
                    PersonIdCardId = table.Column<int>(type: "int", nullable: true),
                    IdentityType = table.Column<int>(type: "int", nullable: false),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<long>(type: "bigint", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddressId = table.Column<int>(type: "int", nullable: true),
                    CorrespondenceAddressId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Customers_AddressCorrespondence_CorrespondenceAddressId",
                        column: x => x.CorrespondenceAddressId,
                        principalTable: "AddressCorrespondence",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Customers_Email_EmailId",
                        column: x => x.EmailId,
                        principalTable: "Email",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Customers_IdCard_PersonIdCardId",
                        column: x => x.PersonIdCardId,
                        principalTable: "IdCard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_PostCode_Code",
                table: "Address",
                column: "PostCode_Code",
                unique: true,
                filter: "[PostCode_Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AddressCorrespondence_PostCode_Code",
                table: "AddressCorrespondence",
                column: "PostCode_Code",
                unique: true,
                filter: "[PostCode_Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_AddressId",
                table: "Customers",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CorrespondenceAddressId",
                table: "Customers",
                column: "CorrespondenceAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_EmailId",
                table: "Customers",
                column: "EmailId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_PersonIdCardId",
                table: "Customers",
                column: "PersonIdCardId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "AddressCorrespondence");

            migrationBuilder.DropTable(
                name: "Email");

            migrationBuilder.DropTable(
                name: "IdCard");
        }
    }
}
