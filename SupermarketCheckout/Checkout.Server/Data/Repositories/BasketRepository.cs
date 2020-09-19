using Microsoft.EntityFrameworkCore;
using Checkout.Server.Data.Contexts;
using Checkout.Server.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Internal;
using Z.EntityFramework.Plus;

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

        public Task<Basket> GetBasket(Guid id) =>
            _context.Baskets.FirstOrDefaultAsync(x => x.Id == id);

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

        public void CheckoutBasket(Basket basketEntity, decimal totalPrice, ICollection<ReceiptItem> receiptItems)
        {
            if (basketEntity == null)
            {
                throw new ArgumentNullException(nameof(basketEntity));
            }
            if (receiptItems == null)
            {
                throw new ArgumentNullException(nameof(receiptItems));
            }
            if (!receiptItems.Any())
            {
                throw new CheckoutException("Cannot checkout an empty basket");
            }

            basketEntity.Status = Entities.Enums.BasketStatus.Checkout;
            basketEntity.TotalPrice = totalPrice;
            basketEntity.ClosedOn = DateTime.Now;
            basketEntity.ReceiptItems = receiptItems;
        }

        public void ReopenBasket(Basket basketEntity)
        {
            if (basketEntity == null)
            {
                throw new ArgumentNullException(nameof(basketEntity));
            }
            basketEntity.Status = Entities.Enums.BasketStatus.Active;
        }

        public Task Save() =>
            _context.SaveChangesAsync();
    }
}
