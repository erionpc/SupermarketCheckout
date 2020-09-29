using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Checkout.Server.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SKU = table.Column<string>(maxLength: 30, nullable: false),
                    Description = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shops",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shops", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemPrices",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ItemId = table.Column<Guid>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    ValidFrom = table.Column<DateTime>(nullable: false),
                    ValidTo = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemPrices_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PointsOfSale",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ShopId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointsOfSale", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PointsOfSale_Shops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Shops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Baskets",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PosId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Baskets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Baskets_PointsOfSale_PosId",
                        column: x => x.PosId,
                        principalTable: "PointsOfSale",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BasketItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BasketId = table.Column<Guid>(nullable: false),
                    ItemId = table.Column<Guid>(nullable: false),
                    AddedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasketItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BasketItems_Baskets_BasketId",
                        column: x => x.BasketId,
                        principalTable: "Baskets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BasketItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Description", "SKU" },
                values: new object[,]
                {
                    { new Guid("bfe604d6-9410-415e-9450-f8df8d3777b0"), "Apple", "A" },
                    { new Guid("67b729b7-f659-4541-82a9-a6f1a005d99c"), "Banana", "B" },
                    { new Guid("fa262d42-6318-40a2-8a9e-12f3e32c270a"), "Carrot", "C" },
                    { new Guid("de5abc1b-4eff-495b-96c3-ae5ad282c3a3"), "Dragon fruit", "D" }
                });

            migrationBuilder.InsertData(
                table: "Shops",
                columns: new[] { "Id", "Description" },
                values: new object[] { new Guid("10073eed-e42e-4e7a-b53d-8731c26c729e"), "The ABCD shop" });

            migrationBuilder.InsertData(
                table: "ItemPrices",
                columns: new[] { "Id", "Amount", "ItemId", "Quantity", "ValidFrom", "ValidTo" },
                values: new object[,]
                {
                    { new Guid("e845c62a-11d5-41e4-ae14-391f54c2f0e7"), 50m, new Guid("bfe604d6-9410-415e-9450-f8df8d3777b0"), 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999) },
                    { new Guid("8f856f06-bf92-4ac9-bf7d-b1cb55302c7d"), 130m, new Guid("bfe604d6-9410-415e-9450-f8df8d3777b0"), 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999) },
                    { new Guid("2305f9e8-4682-4142-a18a-4fe6e502b76f"), 160m, new Guid("bfe604d6-9410-415e-9450-f8df8d3777b0"), 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999) },
                    { new Guid("9420070c-91c4-470a-85cd-3b8112d62916"), 180m, new Guid("bfe604d6-9410-415e-9450-f8df8d3777b0"), 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999) },
                    { new Guid("45ecf056-6690-4260-ab48-e52b62cc3274"), 30m, new Guid("67b729b7-f659-4541-82a9-a6f1a005d99c"), 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999) },
                    { new Guid("020f0702-a802-4832-ba3a-431ecb55856e"), 45m, new Guid("67b729b7-f659-4541-82a9-a6f1a005d99c"), 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999) },
                    { new Guid("afff9c47-d98e-4c6a-a995-a39ebeda8393"), 20m, new Guid("fa262d42-6318-40a2-8a9e-12f3e32c270a"), 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999) },
                    { new Guid("0dce5c00-dd37-4ff6-aaae-e5c51dda3f4c"), 15m, new Guid("de5abc1b-4eff-495b-96c3-ae5ad282c3a3"), 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999) }
                });

            migrationBuilder.InsertData(
                table: "PointsOfSale",
                columns: new[] { "Id", "ShopId" },
                values: new object[] { new Guid("59cc636e-7359-4694-aaec-b3eb0f9023e1"), new Guid("10073eed-e42e-4e7a-b53d-8731c26c729e") });

            migrationBuilder.CreateIndex(
                name: "IX_BasketItems_BasketId",
                table: "BasketItems",
                column: "BasketId");

            migrationBuilder.CreateIndex(
                name: "IX_BasketItems_ItemId",
                table: "BasketItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Baskets_PosId",
                table: "Baskets",
                column: "PosId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemPrices_ItemId",
                table: "ItemPrices",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PointsOfSale_ShopId",
                table: "PointsOfSale",
                column: "ShopId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BasketItems");

            migrationBuilder.DropTable(
                name: "ItemPrices");

            migrationBuilder.DropTable(
                name: "Baskets");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "PointsOfSale");

            migrationBuilder.DropTable(
                name: "Shops");
        }
    }
}
