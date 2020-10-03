using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Checkout.Identity.Services
{
    public interface ISecurityTokenService
    {
        IEnumerable<Claim> GetSecurityTokenClaims();
        
        SecurityToken GetSecurityToken(IEnumerable<Claim> authClaims);

        string SerializeSecurityToken(SecurityToken token);
    }
}
