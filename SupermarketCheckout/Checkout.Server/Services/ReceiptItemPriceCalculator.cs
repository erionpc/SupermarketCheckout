using Checkout.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Checkout.Server.Services
{
    public class ReceiptItemPriceCalculator : IReceiptItemPriceCalculator
    {
        private ICollection<ItemPriceDto> _itemPrices;

        /// <summary>
        /// Returns the price per quantity. It workes recursively by building up the total by largest quantity price defined.
        /// </summary>
        /// <param name="prices">List of item prices</param>
        /// <param name="quantity">Quantity of items</param>
        /// <returns>Price per quantity and eventual offer applied</returns>
        public Tuple<decimal, string> CalculateReceiptItemPrice(ICollection<ItemPriceDto> prices, int quantity)
        {
            if (!prices?.Any() ?? true)
            {
                throw new CheckoutException("prices not defined");
            }

            if (quantity == 0)
            {
                return new Tuple<decimal, string>(0, null);
            }

            // either the quantity is 1 or there is just one price (the unit price)
            if (quantity == 1 || (prices.Count == 1 && prices.First().Quantity == 1))
            {
                return new Tuple<decimal, string>(prices.Single(p => p.Quantity == 1).Amount * quantity, null);
            }

            // there is a price defined for the exact quantity of items in the basket and this is larger than 1
            var priceForExactQuantity = prices.FirstOrDefault(p => p.Quantity == quantity);
            if (priceForExactQuantity != null)
            {
                return new Tuple<decimal, string>(priceForExactQuantity.Amount, $"{quantity} for {priceForExactQuantity.Amount}");
            }

            _itemPrices = new List<ItemPriceDto>(prices);
            decimal price = 0;
            string offer = null;
            getPriceByLargestQuantity(_itemPrices, quantity, ref price, ref offer);

            return new Tuple<decimal, string>(price, offer);
        }

        /// <summary>
        /// Calculates the price by recursively reducing the internal collection of prices while taking the price for the largest quantity, until it gets to the unit price.
        /// </summary>
        /// <param name="prices">Collection of item prices</param>
        /// <param name="quantity">Quantity to calculate for</param>
        /// <param name="price">Price for the current iteration</param>
        /// <param name="offer">Eventual offer detected</param>
        /// <returns>Total price</returns>
        private void getPriceByLargestQuantity(ICollection<ItemPriceDto> prices, int quantity, ref decimal price, ref string offer)
        {
            // attempt to find the price for the exact quantity
            var priceForExactQuantity = prices.FirstOrDefault(p => p.Quantity == quantity);
            if (priceForExactQuantity != null)
            {
                price += priceForExactQuantity.Amount;
                if (quantity > 1)
                {
                    setOfferText(quantity, priceForExactQuantity.Amount, ref offer);
                }                
                return;
            }

            // if the price for the largest quantity inferior to the basket quantity isn't found or it is found and it's the unit price, then we can just return the unit price multiplied by the basket quantity
            var priceForLargestQuantity = prices.OrderByDescending(p => p.Quantity).FirstOrDefault(p => p.Quantity < quantity);
            if (priceForLargestQuantity == null || priceForLargestQuantity.Quantity == 1)
            {
                price += prices.Single(p => p.Quantity == 1).Amount * quantity;
                return;
            }

            setOfferText(priceForLargestQuantity.Quantity, priceForLargestQuantity.Amount, ref offer);

            // if the quantity of the largest price is a dividend of the basket quantity, then this can be used to calculate the total
            if (quantity % priceForLargestQuantity.Quantity == 0)
            {
                price += priceForLargestQuantity.Amount * quantity / priceForLargestQuantity.Quantity;
                return;
            }

            // increment the price with what's been found so far and go for another iteration
            var numberOfLargestQuantityOffers = (int)Math.Floor((decimal) quantity / priceForLargestQuantity.Quantity);
            price += priceForLargestQuantity.Amount * numberOfLargestQuantityOffers;
            quantity -= priceForLargestQuantity.Quantity * numberOfLargestQuantityOffers;
            prices.Remove(priceForLargestQuantity);

            getPriceByLargestQuantity(prices, quantity, ref price, ref offer);
        }

        private static void setOfferText(int quantity, decimal amount, ref string offer) => 
            offer = offer != null ? "multiple offers applied" : $"{quantity} for {amount}";
    }
}
