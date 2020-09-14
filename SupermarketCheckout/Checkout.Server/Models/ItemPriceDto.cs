using System;

namespace Checkout.Server.Models
{
    public class ItemPriceDto
    {
        public Guid Id { get; set; }

        public int Quantity { get; set; } = 1;

        public decimal Amount { get; set; }

        public DateTime ValidFrom { get; set; } = DateTime.MinValue;

        public DateTime ValidTo { get; set; } = DateTime.MaxValue;
    }
}
