using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.Server.Data.Entities.Enums
{
    public enum BasketStatus
    {
        Active = 1,         // can add items
        Checkout = 2,       // can't add any more items
        Paid = 3,           // paid
        Cancelled = 4       // basket cancelled
    }
}
