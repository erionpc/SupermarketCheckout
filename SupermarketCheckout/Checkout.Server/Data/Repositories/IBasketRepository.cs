using Checkout.Server.Data.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.Server.Data.Repositories
{
    public interface IBasketRepository
    {
        ValueTask<EntityEntry<Basket>> CreateBasket(Basket basket);

        ValueTask<EntityEntry<BasketItem>> AddToBasket(BasketItem basketItem, Guid basketId, Guid itemId);

        Task<bool> Exists(Guid id);

        Task<Basket> GetFullBasket(Guid id);

        Task Save();
    }
}
