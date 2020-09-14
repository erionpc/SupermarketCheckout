using Checkout.Server.Models;
using System;
using System.Collections.Generic;

namespace Checkout.Server.Services
{
    public interface ICheckoutService
    {
        ReceiptDto CreateReceipt(ICollection<BasketItemDto> basketItems, Guid posId);
    }
}
