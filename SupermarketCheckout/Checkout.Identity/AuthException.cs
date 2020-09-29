using System;

namespace Checkout.Identity
{
    public class AuthException : Exception
    {
        public AuthException(string message) : base(message)
        {
        }
    }
}
