using Microsoft.EntityFrameworkCore;
using Checkout.Server.Data.Contexts;
using Checkout.Server.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Internal;

namespace Checkout.Server.Data.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly SupermarketDbContext _context;

        public BasketRepository(SupermarketDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task<bool> Exists(Guid id) =>
            _context.Baskets.AnyAsync(x => x.Id == id);

        public Task<Basket> GetFullBasket(Guid id) =>
            _context.Baskets.Include(x => x.BasketItems)
                            .ThenInclude(x => x.Item)
                            .ThenInclude(x => x.Prices).FirstOrDefaultAsync(x => x.Id == id);

        public ValueTask<EntityEntry<Basket>> CreateBasket(Basket basket)
        {
            if (basket == null)
            {
                throw new ArgumentNullException(nameof(basket));
            }

            return _context.Baskets.AddAsync(basket);
        }

        public ValueTask<EntityEntry<BasketItem>> AddToBasket(BasketItem basketItem, Guid basketId, Guid itemId)
        {
            if (basketItem == null)
            {
                throw new ArgumentNullException(nameof(basketItem));
            }
            if (basketId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(basketId));
            }
            if (itemId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(itemId));
            }

            basketItem.BasketId = basketId;
            basketItem.ItemId = itemId;
            basketItem.Item = null;

            return _context.BasketItems.AddAsync(basketItem);
        }

        public Task Save() =>
            _context.SaveChangesAsync();
    }
}
