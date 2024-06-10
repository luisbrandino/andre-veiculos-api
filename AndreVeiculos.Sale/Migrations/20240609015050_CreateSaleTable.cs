using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AndreVeiculos.Sale.Migrations
{
    public partial class CreateSaleTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sale",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarLicensePlate = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClientIdentificationNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EmployeeIdentificationNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PaymentId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SoldAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sale", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sale_Car_CarLicensePlate",
                        column: x => x.CarLicensePlate,
                        principalTable: "Car",
                        principalColumn: "LicensePlate",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sale_Client_ClientIdentificationNumber",
                        column: x => x.ClientIdentificationNumber,
                        principalTable: "Client",
                        principalColumn: "IdentificationNumber",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sale_Employee_EmployeeIdentificationNumber",
                        column: x => x.EmployeeIdentificationNumber,
                        principalTable: "Employee",
                        principalColumn: "IdentificationNumber");
                    table.ForeignKey(
                        name: "FK_Sale_Payment_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sale_CarLicensePlate",
                table: "Sale",
                column: "CarLicensePlate");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_ClientIdentificationNumber",
                table: "Sale",
                column: "ClientIdentificationNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_EmployeeIdentificationNumber",
                table: "Sale",
                column: "EmployeeIdentificationNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_PaymentId",
                table: "Sale",
                column: "PaymentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sale");
        }
    }
}
