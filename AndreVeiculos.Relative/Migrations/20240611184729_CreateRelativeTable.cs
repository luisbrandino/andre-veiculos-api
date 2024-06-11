using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AndreVeiculos.Relative.Migrations
{
    public partial class CreateRelativeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Relative",
                columns: table => new
                {
                    IdentificationNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClientIdentificationNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddressId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relative", x => x.IdentificationNumber);
                    table.ForeignKey(
                        name: "FK_Relative_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Relative_Client_ClientIdentificationNumber",
                        column: x => x.ClientIdentificationNumber,
                        principalTable: "Client",
                        principalColumn: "IdentificationNumber");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Relative_AddressId",
                table: "Relative",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Relative_ClientIdentificationNumber",
                table: "Relative",
                column: "ClientIdentificationNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Relative");

        }
    }
}
