using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Checkout.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Checkout.Identity.Services;
using Checkout.Shared;

namespace Checkout.Identity.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly ISecurityTokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthenticationController(IIdentityService authService,
                                        ISecurityTokenService tokenService,
                                        IConfiguration configuration,
                                        IMapper mapper)
        {
            this._identityService = authService ??
                throw new ArgumentNullException(nameof(authService));
            this._tokenService = tokenService ??
                throw new ArgumentNullException(nameof(tokenService));
            _configuration = configuration ??
                throw new ArgumentNullException(nameof(configuration));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var user = await _identityService.FindByNameAsync(model.Username);
            if (user == null)
            {
                return Unauthorized();
            }

            var passwordCheck = await _identityService.CheckPasswordAsync(user, model.Password);
            if (!passwordCheck)
            {
                return Unauthorized();
            }

            var userRoles = await _identityService.GetRolesAsync(user);

            var authClaims = new List<Claim>
            { 
                new Claim(ClaimTypes.Name, user.UserName)
            };
            
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var securityTokenClaims = _tokenService.GetSecurityTokenClaims();
            if (securityTokenClaims?.Any() ?? false)
            {
                authClaims.AddRange(securityTokenClaims);
            }

            var token = _tokenService.GetSecurityToken(authClaims);

            return Ok(new
            {
                token = _tokenService.SerializeSecurityToken(token),
                expiration = token.ValidTo
            });
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationInfo registrationInput)
        {
            var registrationResult = await registerUser(registrationInput);
            if (registrationResult.Item1.GetType() != typeof(OkResult))
            {
                return registrationResult.Item1;
            }

            if (await _identityService.RoleExistsAsync(UserRoles.Pos))
            {
                await _identityService.AddToRoleAsync(registrationResult.Item2, UserRoles.Pos);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new IdentityResponseDto(AuthResult.Error, "Role not defined!"));
            }

            return Ok(new IdentityResponseDto(AuthResult.Success, "User created successfully!"));
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegistrationInfo registrationInput)
        {
            var inputValidationResult = await registerUser(registrationInput);
            if (inputValidationResult.Item1.GetType() != typeof(OkResult))
            {
                return inputValidationResult.Item1;
            }
            
            if (await _identityService.RoleExistsAsync(UserRoles.Admin))
            {
                await _identityService.AddToRoleAsync(inputValidationResult.Item2, UserRoles.Admin);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new IdentityResponseDto(AuthResult.Error, "Role not defined!"));
            }

            return Ok(new IdentityResponseDto(AuthResult.Success, "User created successfully!"));
        }

        private async Task<Tuple<IActionResult, ApplicationUser>> registerUser(RegistrationInfo registrationInput)
        {
            var userExists = await _identityService.FindByNameAsync(registrationInput.Username);
            if (userExists != null)
            {
                return new Tuple<IActionResult, ApplicationUser>(StatusCode(StatusCodes.Status500InternalServerError, new IdentityResponseDto(AuthResult.Error, "User already exists!")), null);
            }

            var user = _mapper.Map<RegistrationInfo, ApplicationUser>(registrationInput);
            var result = await _identityService.CreateAsync(user, registrationInput.Password);
            if (result.StatusEnum != AuthResult.Success)
            {
                return new Tuple<IActionResult, ApplicationUser>(StatusCode(StatusCodes.Status500InternalServerError, new IdentityResponseDto(AuthResult.Error, result.Message)), null);
            }

            return new Tuple<IActionResult, ApplicationUser>(Ok(), user);
        }
    }
}
