using api.Application.Dtos.AuthDtos;
using api.Infrastructure.Config;
using API.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JWTBearerTokenSettings jwtBearerTokenSettings;
        private readonly UserManager<ApplicationUser> userManager;

        public AuthController(IOptions<JWTBearerTokenSettings> jwtTokenOptions, UserManager<ApplicationUser> userManager)
        {
            this.jwtBearerTokenSettings = jwtTokenOptions.Value;
            this.userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto userDetails)
        {
            if (!ModelState.IsValid || userDetails == null)
            {
                return new BadRequestObjectResult(new
                {
                    Message = "User Registration Failed"
                });
            }

            var applicationUser = new ApplicationUser
            {
                UserName = userDetails.Username,
                Email = userDetails.Email,
            };

            applicationUser.OnPersist();

            var result = await userManager.CreateAsync(applicationUser, userDetails.Password);

            if (result.Succeeded)
            {
                return Ok("User registered successfully.");
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var token = GenerateToken(user);
                return Ok(new { token });
            }
            return Unauthorized();
        }

        private string GenerateToken(ApplicationUser applicationUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtBearerTokenSettings.SecretKey);

            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, applicationUser.UserName.ToString()),
                    new Claim(ClaimTypes.Email, applicationUser.Email)
                }),
                Expires = now.AddSeconds(jwtBearerTokenSettings.ExpiryTimeInSeconds),
                NotBefore = now,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = jwtBearerTokenSettings.Audience,
                Issuer = jwtBearerTokenSettings.Issuer
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }




}