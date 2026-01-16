using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebStore.Migrations
{
    /// <inheritdoc />
    public partial class AddCityToCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CityId",
                schema: "webstore",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CityId",
                schema: "webstore",
                table: "Customers",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Cities_CityId",
                schema: "webstore",
                table: "Customers",
                column: "CityId",
                principalSchema: "webstore",
                principalTable: "Cities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Cities_CityId",
                schema: "webstore",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_CityId",
                schema: "webstore",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CityId",
                schema: "webstore",
                table: "Customers");
        }
    }
}
