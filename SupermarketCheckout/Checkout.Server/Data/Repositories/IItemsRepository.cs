using Checkout.Server.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.Server.Data.Repositories
{
    public interface IItemsRepository
    {
        Task<List<Item>> GetItems();

        Task<Item> GetItemBySKU(string sku);
    }
}
