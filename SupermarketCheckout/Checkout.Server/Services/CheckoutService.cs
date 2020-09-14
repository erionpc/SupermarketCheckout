using Checkout.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Checkout.Server.Services
{
    public class CheckoutService : ICheckoutService
    {
        private IReceiptItemPriceCalculator _priceCalculator;

        public CheckoutService(IReceiptItemPriceCalculator priceCalculator)
        {
            _priceCalculator = priceCalculator ?? 
                throw new ArgumentException(nameof(priceCalculator));
        }

        public ReceiptDto CreateReceipt(ICollection<BasketItemDto> basketItems, Guid posId)
        {
            if (basketItems == null)
                return null;

            if (basketItems.Any(x => x.Item == null))
                throw new CheckoutException("Inventory not loaded");

            if (basketItems.Any(x => !(x.Item.Prices?.Any() ?? false)))
                throw new CheckoutException("Prices not loaded");

            if (basketItems.Any(x => !x.Item.Prices.Any(x => x.Quantity == 1)))
                throw new CheckoutException("Unit prices not specified");

            var receiptItems = basketItems.GroupBy(x => x.Item.SKU)
                                          .Select(x => new
                                          {
                                              SKU = x.Key,
                                              Description = x.First().Item.Description,
                                              Quantity = x.Count(),
                                              PriceAndOffer = _priceCalculator.CalculateReceiptItemPrice(x.First().Item.Prices, x.Count())
                                          })
                                          .Select(x => new ReceiptItemDto()
                                          {
                                              SKU = x.SKU,
                                              Description = x.Description,
                                              Quantity = x.Quantity,
                                              Price = x.PriceAndOffer.Item1,
                                              OfferText = x.PriceAndOffer.Item2
                                          });

            var total = receiptItems.Sum(x => x.Price);

            return new ReceiptDto()
            {
                PosId = posId,
                Items = receiptItems.ToList(),
                TotalPrice = total
            };
        }
    }
}
