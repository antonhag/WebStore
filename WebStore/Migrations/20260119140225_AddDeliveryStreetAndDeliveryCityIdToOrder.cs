using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebStore.Migrations
{
    /// <inheritdoc />
    public partial class AddDeliveryStreetAndDeliveryCityIdToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeliveryCityId",
                schema: "webstore",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryStreet",
                schema: "webstore",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DeliveryCityId",
                schema: "webstore",
                table: "Orders",
                column: "DeliveryCityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Cities_DeliveryCityId",
                schema: "webstore",
                table: "Orders",
                column: "DeliveryCityId",
                principalSchema: "webstore",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Cities_DeliveryCityId",
                schema: "webstore",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DeliveryCityId",
                schema: "webstore",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryCityId",
                schema: "webstore",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryStreet",
                schema: "webstore",
                table: "Orders");
        }
    }
}
