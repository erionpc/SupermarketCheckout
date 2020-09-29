using AutoMapper;
using Checkout.Identity.Models;
using Checkout.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.Identity.Services
{
    public class MicrosoftIdentityAuthService : IAuthService
    {
        private readonly UserManager<ApplicationUserDto> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private bool disposedValue;

        public MicrosoftIdentityAuthService(UserManager<ApplicationUserDto> userManager,
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

        public Task<ApplicationUserDto> FindByNameAsync(string username) =>
            _userManager.FindByNameAsync(username);

        public Task<bool> CheckPasswordAsync(ApplicationUserDto user, string password) =>
            _userManager.CheckPasswordAsync(user, password);

        public Task<IList<string>> GetRolesAsync(ApplicationUserDto user) =>
            _userManager.GetRolesAsync(user);

        public Task<bool> RoleExistsAsync(string role) =>
            _roleManager.RoleExistsAsync(role);

        public async Task<AuthResponseDto> AddToRoleAsync(ApplicationUserDto user, string role) =>
            _mapper.Map<IdentityResult, AuthResponseDto>(await _userManager.AddToRoleAsync(user, role));

        public async Task<AuthResponseDto> CreateAsync(ApplicationUserDto user, string password) =>
            _mapper.Map<IdentityResult, AuthResponseDto>(await _userManager.CreateAsync(user, password));

        public async Task InitialiseAuthData()
        {
            await initialiseRoles();
            await initialiseUsers();            
        }

        private async Task initialiseRoles()
        {
            try
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
            catch (Exception)
            {
                throw;
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
                var user = new ApplicationUserDto()
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
