using AutoMapper;
using Checkout.Identity.Data.Contexts;
using Checkout.Identity.Models;
using Checkout.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.Identity.Services
{
    public class MicrosoftIdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private bool disposedValue;

        public MicrosoftIdentityService(UserManager<IdentityUser> userManager,
                                        RoleManager<IdentityRole> roleManager,
                                        IConfiguration configuration,
                                        IMapper mapper)
        {
            this._userManager = userManager ??
                throw new ArgumentNullException(nameof(userManager));
            this._roleManager = roleManager ??
                throw new ArgumentNullException(nameof(roleManager));
            _configuration = configuration ??
                throw new ArgumentNullException(nameof(configuration));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ApplicationUser> FindByNameAsync(string username) =>
            _mapper.Map<IdentityUser, ApplicationUser>(await _userManager.FindByNameAsync(username));

        public Task<bool> CheckPasswordAsync(ApplicationUser user, string password) =>
            _userManager.CheckPasswordAsync(_mapper.Map<ApplicationUser, IdentityUser>(user), password);

        public Task<IList<string>> GetRolesAsync(ApplicationUser user) =>
            _userManager.GetRolesAsync(_mapper.Map<ApplicationUser, IdentityUser>(user));

        public Task<bool> RoleExistsAsync(string role) =>
            _roleManager.RoleExistsAsync(role);

        public async Task<IdentityResponseDto> AddToRoleAsync(ApplicationUser user, string role)
        {
            // to avoid EF tracking of the user
            var identityUser = await _userManager.FindByNameAsync(user.UserName);
            var identityResult = await _userManager.AddToRoleAsync(identityUser, role);

            return _mapper.Map<IdentityResult, IdentityResponseDto>(identityResult);
        }

        public async Task<IdentityResponseDto> CreateAsync(ApplicationUser user, string password)
        {
            var identityUser = _mapper.Map<ApplicationUser, IdentityUser>(user);
            var identityResult = await _userManager.CreateAsync(identityUser, password);

            return _mapper.Map<IdentityResult, IdentityResponseDto>(identityResult);
        }

        public async Task InitialiseAuthData()
        {
            await initialiseRoles();
            await initialiseUsers();            
        }

        private async Task initialiseRoles()
        {
            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            }
            if (!await _roleManager.RoleExistsAsync(UserRoles.Pos))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Pos));
            }
        }

        private async Task initialiseUsers()
        {
            await initialiseUser(_configuration["Users:Admin:Username"], _configuration["Users:Admin:Password"], _configuration["Users:Admin:Email"], UserRoles.Admin);

            await initialiseUser(_configuration["Users:POS:Username"], _configuration["Users:POS:Password"], _configuration["Users:POS:Email"], UserRoles.Pos);
        }

        private async Task initialiseUser(string username, string password, string email, string role)
        {
            var userExists = await _userManager.FindByNameAsync(username);
            
            if (userExists == null)
            {
                var user = new IdentityUser()
                {
                    UserName = username,
                    Email = email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                };
                var result = await _userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    throw new AuthException($"User '{username}' creation failed. Error: {string.Join(",", result.Errors?.Select(e => e.Description))}");
                }

                if (await _roleManager.RoleExistsAsync(role))
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
            }
        }

        #region Disposable implementation
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~MicrosoftIdentityAuthService()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
