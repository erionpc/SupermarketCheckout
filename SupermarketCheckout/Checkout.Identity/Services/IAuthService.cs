using Checkout.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.Identity.Services
{
    public interface IAuthService : IDisposable
    {
        public Task InitialiseAuthData();

        Task<ApplicationUserDto> FindByNameAsync(string username);

        Task<bool> CheckPasswordAsync(ApplicationUserDto user, string password);

        Task<IList<string>> GetRolesAsync(ApplicationUserDto user);

        Task<bool> RoleExistsAsync(string role);

        Task<AuthResponseDto> AddToRoleAsync(ApplicationUserDto user, string role);

        Task<AuthResponseDto> CreateAsync(ApplicationUserDto user, string password);
    }
}
