using System;
using System.Collections.Generic;

namespace Checkout.Server.Models
{
    public class ItemDto
    {
        public Guid Id { get; set; }

        public string SKU { get; set; }

        public string Description { get; set; }

        public ICollection<ItemPriceDto> Prices { get; set; }
    }
}
