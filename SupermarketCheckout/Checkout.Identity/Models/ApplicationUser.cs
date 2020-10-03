using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Checkout.Identity.Models
{
    public class ApplicationUser
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }

        public string Id { get; set; }
    }
}
