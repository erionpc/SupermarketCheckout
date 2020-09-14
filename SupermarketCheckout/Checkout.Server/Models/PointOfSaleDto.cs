using System;
using System.Collections.Generic;

namespace Checkout.Server.Models
{
    public class PointOfSaleDto
    {
        public Guid Id { get; set; }

        public Guid ShopId { get; set; }

        public ICollection<BasketDto> Baskets { get; set; } 
            = new List<BasketDto>();
    }
}
