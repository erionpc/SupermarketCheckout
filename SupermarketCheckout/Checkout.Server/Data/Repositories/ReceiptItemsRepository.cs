using Microsoft.EntityFrameworkCore;
using Checkout.Server.Data.Contexts;
using Checkout.Server.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;
using System.Security.Cryptography.X509Certificates;

namespace Checkout.Server.Data.Repositories
{
    public class ReceiptItemsRepository : IReceiptItemsRepository
    {
        private readonly SupermarketDbContext _context;

        public ReceiptItemsRepository(SupermarketDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task DeleteBasketReceiptItems(Basket basket)
        {
            if (basket == null)
            {
                throw new ArgumentNullException(nameof(basket));
            }

            return _context.ReceiptItems.Where(x => x.BasketId == basket.Id).DeleteFromQueryAsync();
        }
    }
}
