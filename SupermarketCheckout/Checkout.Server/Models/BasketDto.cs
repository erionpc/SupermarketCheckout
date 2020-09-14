using System;
using System.Collections.Generic;

namespace Checkout.Server.Models
{
    public class BasketDto
    {
        public Guid Id { get; set; }

        public Guid PosId { get; set; }

        public DateTime CreatedOn { get; set; }

        public ICollection<BasketItemDto> BasketItems { get; set; } 
            = new List<BasketItemDto>();


    }
}
