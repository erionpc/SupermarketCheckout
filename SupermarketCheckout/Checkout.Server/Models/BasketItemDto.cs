using System;

namespace Checkout.Server.Models
{
    public class BasketItemDto
    {
        public Guid Id { get; set; }

        public Guid BasketId { get; set; }

        public ItemDto Item { get; set; }
        
        public DateTime AddedOn { get; set; }
    }
}
