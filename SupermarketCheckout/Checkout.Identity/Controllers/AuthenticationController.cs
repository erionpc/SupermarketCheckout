using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
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
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthenticationController(IAuthService authService, 
                                      IConfiguration configuration,
                                      IMapper mapper)
        {
            this._authService = authService ??
                throw new ArgumentNullException(nameof(authService));
            _configuration = configuration ??
                throw new ArgumentNullException(nameof(configuration));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var user = await _authService.FindByNameAsync(model.Username);
            if (user == null)
            {
                return Unauthorized();
            }

            var passwordCheck = await _authService.CheckPasswordAsync(user, model.Password);
            if (!passwordCheck)
            {
                return Unauthorized();
            }

            var userRoles = await _authService.GetRolesAsync(user);

            var authClaims = new List<Claim>
            { 
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(int.Parse(_configuration["JWT:TokenExpiryMinutes"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationInfo registrationInput)
        {
            var registrationResult = await registerUser(registrationInput);
            if (registrationResult.Item1.GetType() != typeof(OkResult))
            {
                return registrationResult.Item1;
            }

            if (await _authService.RoleExistsAsync(UserRoles.Pos))
            {
                await _authService.AddToRoleAsync(registrationResult.Item2, UserRoles.Pos);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new AuthResponseDto(AuthResult.Error, "Role not defined!"));
            }

            return Ok(new AuthResponseDto(AuthResult.Success, "User created successfully!"));
        }

        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegistrationInfo registrationInput)
        {
            var inputValidationResult = await registerUser(registrationInput);
            if (inputValidationResult.Item1.GetType() != typeof(OkResult))
            {
                return inputValidationResult.Item1;
            }
            
            if (await _authService.RoleExistsAsync(UserRoles.Admin))
            {
                await _authService.AddToRoleAsync(inputValidationResult.Item2, UserRoles.Admin);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new AuthResponseDto(AuthResult.Error, "Role not defined!"));
            }

            return Ok(new AuthResponseDto(AuthResult.Success, "User created successfully!"));
        }

        private async Task<Tuple<IActionResult, ApplicationUserDto>> registerUser(RegistrationInfo registrationInput)
        {
            var userExists = await _authService.FindByNameAsync(registrationInput.Username);
            if (userExists != null)
            {
                return new Tuple<IActionResult, ApplicationUserDto>(StatusCode(StatusCodes.Status500InternalServerError, new AuthResponseDto(AuthResult.Error, "User already exists!")), null);
            }

            var user = _mapper.Map<RegistrationInfo, ApplicationUserDto>(registrationInput);
            var result = await _authService.CreateAsync(user, registrationInput.Password);
            if (result.StatusEnum != AuthResult.Success)
            {
                return new Tuple<IActionResult, ApplicationUserDto>(StatusCode(StatusCodes.Status500InternalServerError, new AuthResponseDto(AuthResult.Error, result.Message)), null);
            }

            return new Tuple<IActionResult, ApplicationUserDto>(Ok(), user);
        }
    }
}
