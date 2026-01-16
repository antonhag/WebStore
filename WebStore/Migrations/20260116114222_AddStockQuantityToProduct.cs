using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebStore.Migrations
{
    /// <inheritdoc />
    public partial class AddStockQuantityToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StockQuantity",
                schema: "webstore",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StockQuantity",
                schema: "webstore",
                table: "Products");
        }
    }
}
