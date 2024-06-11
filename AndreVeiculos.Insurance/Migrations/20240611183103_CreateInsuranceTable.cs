using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AndreVeiculos.Insurance.Migrations
{
    public partial class CreateInsuranceTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Insurance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientIdentificationNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CarLicensePlate = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DriverIdentificationNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Deductible = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Insurance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Insurance_Car_CarLicensePlate",
                        column: x => x.CarLicensePlate,
                        principalTable: "Car",
                        principalColumn: "LicensePlate",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Insurance_Client_ClientIdentificationNumber",
                        column: x => x.ClientIdentificationNumber,
                        principalTable: "Client",
                        principalColumn: "IdentificationNumber",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Insurance_Driver_DriverIdentificationNumber",
                        column: x => x.DriverIdentificationNumber,
                        principalTable: "Driver",
                        principalColumn: "IdentificationNumber");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Client_AddressId",
                table: "Client",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Insurance_CarLicensePlate",
                table: "Insurance",
                column: "CarLicensePlate");

            migrationBuilder.CreateIndex(
                name: "IX_Insurance_ClientIdentificationNumber",
                table: "Insurance",
                column: "ClientIdentificationNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Insurance_DriverIdentificationNumber",
                table: "Insurance",
                column: "DriverIdentificationNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Insurance");
        }
    }
}
