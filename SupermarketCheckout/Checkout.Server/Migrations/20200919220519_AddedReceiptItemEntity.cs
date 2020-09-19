using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Checkout.Server.Migrations
{
    public partial class AddedReceiptItemEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ClosedOn",
                table: "Baskets",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Baskets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Baskets",
                type: "decimal(9,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "ReceiptItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ItemId = table.Column<Guid>(nullable: false),
                    SKU = table.Column<string>(maxLength: 30, nullable: false),
                    BasketId = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(maxLength: 50, nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    OfferText = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceiptItems_Baskets_BasketId",
                        column: x => x.BasketId,
                        principalTable: "Baskets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceiptItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptItems_BasketId",
                table: "ReceiptItems",
                column: "BasketId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptItems_ItemId",
                table: "ReceiptItems",
                column: "ItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReceiptItems");

            migrationBuilder.DropColumn(
                name: "ClosedOn",
                table: "Baskets");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Baskets");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Baskets");
        }
    }
}
