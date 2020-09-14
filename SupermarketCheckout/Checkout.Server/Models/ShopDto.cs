using System;
using System.Collections.Generic;

namespace Checkout.Server.Models
{
    public class ShopDto
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public ICollection<PointOfSaleDto> PointsOfSale { get; set; } 
            = new List<PointOfSaleDto>();
    }
}
