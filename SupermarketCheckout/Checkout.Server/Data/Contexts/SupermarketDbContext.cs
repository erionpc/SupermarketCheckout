using Microsoft.EntityFrameworkCore;
using Checkout.Server.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.Server.Data.Contexts
{
    public class SupermarketDbContext : DbContext, IDisposable
    {
        public DbSet<Shop> Shops { get; set; }

        public DbSet<PointOfSale> PointsOfSale { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<ItemPrice> ItemPrices { get; set; }

        public DbSet<Basket> Baskets { get; set; }

        public DbSet<BasketItem> BasketItems { get; set; }

        public SupermarketDbContext(DbContextOptions<SupermarketDbContext> options)
           : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // seed the database with demo data
            modelBuilder.Entity<Shop>().HasData(
                new Shop()
                {
                    Id = Guid.Parse("10073eed-e42e-4e7a-b53d-8731c26c729e"),
                    Description = "The ABCD shop"
                });

            modelBuilder.Entity<PointOfSale>().HasData(
                new PointOfSale()
                {
                    Id = Guid.Parse("59cc636e-7359-4694-aaec-b3eb0f9023e1"),
                    ShopId = Guid.Parse("10073eed-e42e-4e7a-b53d-8731c26c729e")
                });

            modelBuilder.Entity<Item>().HasData(
                new Item()
                {
                    Id = Guid.Parse("bfe604d6-9410-415e-9450-f8df8d3777b0"),
                    SKU = "A",
                    Description = "Apple"
                },
                new Item()
                {
                    Id = Guid.Parse("67b729b7-f659-4541-82a9-a6f1a005d99c"),
                    SKU = "B",
                    Description = "Banana"
                },
                new Item()
                {
                    Id = Guid.Parse("fa262d42-6318-40a2-8a9e-12f3e32c270a"),
                    SKU = "C",
                    Description = "Carrot"
                },
                new Item()
                {
                    Id = Guid.Parse("de5abc1b-4eff-495b-96c3-ae5ad282c3a3"),
                    SKU = "D",
                    Description = "Dragon fruit"
                });

            modelBuilder.Entity<ItemPrice>().HasData(
                new ItemPrice()
                {
                    Id = Guid.Parse("e845c62a-11d5-41e4-ae14-391f54c2f0e7"),
                    ItemId = Guid.Parse("bfe604d6-9410-415e-9450-f8df8d3777b0"),
                    Amount = 50
                },
                new ItemPrice()
                {
                    Id = Guid.Parse("8f856f06-bf92-4ac9-bf7d-b1cb55302c7d"),
                    ItemId = Guid.Parse("bfe604d6-9410-415e-9450-f8df8d3777b0"),
                    Quantity = 3,
                    Amount = 130
                },
                new ItemPrice()
                {
                    Id = Guid.Parse("2305f9e8-4682-4142-a18a-4fe6e502b76f"),
                    ItemId = Guid.Parse("bfe604d6-9410-415e-9450-f8df8d3777b0"),
                    Quantity = 4,
                    Amount = 160
                },
                new ItemPrice()
                {
                    Id = Guid.Parse("9420070c-91c4-470a-85cd-3b8112d62916"),
                    ItemId = Guid.Parse("bfe604d6-9410-415e-9450-f8df8d3777b0"),
                    Quantity = 5,
                    Amount = 180
                },
                new ItemPrice()
                {
                    Id = Guid.Parse("45ecf056-6690-4260-ab48-e52b62cc3274"),
                    ItemId = Guid.Parse("67b729b7-f659-4541-82a9-a6f1a005d99c"),
                    Amount = 30
                },
                new ItemPrice()
                {
                    Id = Guid.Parse("020f0702-a802-4832-ba3a-431ecb55856e"),
                    ItemId = Guid.Parse("67b729b7-f659-4541-82a9-a6f1a005d99c"),
                    Quantity = 2,
                    Amount = 45
                },
                new ItemPrice()
                {
                    Id = Guid.Parse("afff9c47-d98e-4c6a-a995-a39ebeda8393"),
                    ItemId = Guid.Parse("fa262d42-6318-40a2-8a9e-12f3e32c270a"),
                    Amount = 20
                },
                new ItemPrice()
                {
                    Id = Guid.Parse("0dce5c00-dd37-4ff6-aaae-e5c51dda3f4c"),
                    ItemId = Guid.Parse("de5abc1b-4eff-495b-96c3-ae5ad282c3a3"),
                    Amount = 15
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
