using System;
using System.Collections.Generic;

namespace Checkout.Server.Models
{
    public class ReceiptItemDto
    {
        public string SKU { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public string OfferText { get; set; }
    }
}
