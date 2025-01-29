using api.Application.Dtos.AuthDtos;
using api.Infrastructure.Config;
using API.Domain.Interfaces;
using API.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Presentation.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly JWTBearerTokenSettings _jwtBearerTokenSettings;

        public AuthController(IOptions<JWTBearerTokenSettings> jwtTokenOptions, IApplicationUserRepository applicationUserRepository)
        {
            _jwtBearerTokenSettings = jwtTokenOptions.Value;
            _applicationUserRepository = applicationUserRepository;
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

            var result = await _applicationUserRepository.AddAsync(applicationUser, userDetails.Password);

            if (result.Succeeded)
            {
                return Ok("User registered successfully.");
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var user = await _applicationUserRepository.GetByUserNameAsync(model.Username);
            if (user != null && await _applicationUserRepository.CheckPasswordAsync(user, model.Password))
            {
                var token = GenerateToken(user);
                return Ok(new { token });
            }
            return Unauthorized();
        }

        private string GenerateToken(ApplicationUser applicationUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtBearerTokenSettings.SecretKey);

            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, applicationUser.UserName.ToString()),
                    new Claim(ClaimTypes.Email, applicationUser.Email)
                }),
                Expires = now.AddSeconds(_jwtBearerTokenSettings.ExpiryTimeInSeconds),
                NotBefore = now,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _jwtBearerTokenSettings.Audience,
                Issuer = _jwtBearerTokenSettings.Issuer
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }




}