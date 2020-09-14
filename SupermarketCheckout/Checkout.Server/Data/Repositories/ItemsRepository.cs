using Microsoft.EntityFrameworkCore;
using Checkout.Server.Data.Contexts;
using Checkout.Server.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace Checkout.Server.Data.Repositories
{
    public class ItemsRepository : IItemsRepository
    {
        private readonly SupermarketDbContext _context;

        public ItemsRepository(SupermarketDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task<List<Item>> GetItems() =>
            _context.Items.IncludeFilter(x => x.Prices.Where(p => p.ValidFrom <= DateTime.Now && p.ValidTo >= DateTime.Now))
                          .OrderBy(x => x.SKU).ToListAsync();

        public Task<Item> GetItemBySKU(string sku) =>
            _context.Items.FirstOrDefaultAsync(x => x.SKU == sku);
    }
}
