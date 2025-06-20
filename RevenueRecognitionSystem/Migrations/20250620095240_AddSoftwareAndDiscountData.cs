using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RevenueRecognitionSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftwareAndDiscountData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Discount",
                columns: new[] { "IdDiscount", "DiscountType", "EndDate", "Name", "StartDate", "Value" },
                values: new object[] { 1, "temporary", new DateTime(2024, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Black Friday", new DateTime(2024, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 10m });

            migrationBuilder.InsertData(
                table: "Software",
                columns: new[] { "IdSoftware", "BaseYearlyPrice", "Category", "CurrentVersion", "Description", "Name" },
                values: new object[] { 1, 5000m, "Finance", "1.0.0", "Finance software", "AccountingPro" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Discount",
                keyColumn: "IdDiscount",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Software",
                keyColumn: "IdSoftware",
                keyValue: 1);
        }
    }
}
