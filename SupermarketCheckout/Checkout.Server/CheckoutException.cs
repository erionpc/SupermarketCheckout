using System;

namespace Checkout.Server
{
    public class CheckoutException : Exception
    {
        public CheckoutException(string message) : base(message)
        {
        }
    }
}
