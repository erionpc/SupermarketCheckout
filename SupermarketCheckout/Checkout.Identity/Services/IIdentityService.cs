using Checkout.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.Identity.Services
{
    public interface IIdentityService : IDisposable
    {
        public Task InitialiseAuthData();

        Task<ApplicationUser> FindByNameAsync(string username);

        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);

        Task<IList<string>> GetRolesAsync(ApplicationUser user);

        Task<bool> RoleExistsAsync(string role);

        Task<IdentityResponseDto> AddToRoleAsync(ApplicationUser user, string role);

        Task<IdentityResponseDto> CreateAsync(ApplicationUser user, string password);
    }
}
