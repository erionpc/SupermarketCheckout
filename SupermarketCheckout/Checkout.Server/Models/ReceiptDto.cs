using System;
using System.Collections.Generic;

namespace Checkout.Server.Models
{
    public class ReceiptDto
    {
        public Guid PosId { get; internal set; }

        public ICollection<ReceiptItemDto> Items { get; set; } = new List<ReceiptItemDto>();

        public decimal TotalPrice { get; set; }
    }
}
