using System;
using System.Text.Json.Serialization;

namespace Checkout.Server.Models
{
    public class ReceiptItemDto
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        [JsonIgnore]
        public Guid ItemId { get; set; }

        public string SKU { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public string OfferText { get; set; }
    }
}
