using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebStore.Migrations
{
    /// <inheritdoc />
    public partial class AddOrdersPaymentAndDeliveryModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                schema: "webstore",
                table: "CartItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CreditCards",
                schema: "webstore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    CardNumberLast4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CardType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreditCards_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "webstore",
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryOptions",
                schema: "webstore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstimatedTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                schema: "webstore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                schema: "webstore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    PaymentMethodId = table.Column<int>(type: "int", nullable: false),
                    DeliveryOptionId = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "webstore",
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_DeliveryOptions_DeliveryOptionId",
                        column: x => x.DeliveryOptionId,
                        principalSchema: "webstore",
                        principalTable: "DeliveryOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_PaymentMethods_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalSchema: "webstore",
                        principalTable: "PaymentMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_OrderId",
                schema: "webstore",
                table: "CartItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditCards_CustomerId",
                schema: "webstore",
                table: "CreditCards",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                schema: "webstore",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DeliveryOptionId",
                schema: "webstore",
                table: "Orders",
                column: "DeliveryOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PaymentMethodId",
                schema: "webstore",
                table: "Orders",
                column: "PaymentMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Orders_OrderId",
                schema: "webstore",
                table: "CartItems",
                column: "OrderId",
                principalSchema: "webstore",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Orders_OrderId",
                schema: "webstore",
                table: "CartItems");

            migrationBuilder.DropTable(
                name: "CreditCards",
                schema: "webstore");

            migrationBuilder.DropTable(
                name: "Orders",
                schema: "webstore");

            migrationBuilder.DropTable(
                name: "DeliveryOptions",
                schema: "webstore");

            migrationBuilder.DropTable(
                name: "PaymentMethods",
                schema: "webstore");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_OrderId",
                schema: "webstore",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "OrderId",
                schema: "webstore",
                table: "CartItems");
        }
    }
}
