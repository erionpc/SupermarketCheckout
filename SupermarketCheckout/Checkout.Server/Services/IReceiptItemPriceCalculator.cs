using Checkout.Server.Models;
using System;
using System.Collections.Generic;

namespace Checkout.Server.Services
{
    public interface IReceiptItemPriceCalculator
    {
        Tuple<decimal, string> CalculateReceiptItemPrice(ICollection<ItemPriceDto> prices, int quantity);
    }
}
