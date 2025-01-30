using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using api.Application.Dtos.AuthDtos;
using api.Application.Services.ServiceContracts;
using api.Infrastructure.Config;
using API.Domain.Interfaces;
using API.Domain.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace api.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly JWTBearerTokenSettings _jwtBearerTokenSettings;
        private readonly IApplicationUserRepository _userRepository;
        private readonly IRevokedTokenService _tokenRevocationService;
        private readonly IMapper _mapper;



        public AuthService(IApplicationUserRepository userRepository, IOptions<JWTBearerTokenSettings> jwtTokenOptions,
            IRevokedTokenService tokenRevocationService,
            IMapper mapper)
        {
            _jwtBearerTokenSettings = jwtTokenOptions.Value;
            _userRepository = userRepository;
            _tokenRevocationService = tokenRevocationService;
            _mapper = mapper;

        }

        public async Task<string> RegisterAsync(RegisterDto userDetails)
        {
            var user = _mapper.Map<ApplicationUser>(userDetails);


            var result = await _userRepository.AddAsync(user, userDetails.Password);

            if (result.Succeeded)
            {
                return "User registered successfully.";
            }

            throw new Exception("User registration failed.");
        }

        private string GenerateToken(ApplicationUser applicationUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtBearerTokenSettings.SecretKey);

            var now = DateTime.Now;
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

        public async Task<string> LoginAsync(LoginDto model)
        {
            var user = await _userRepository.GetByUserNameAsync(model.Username);
            if (user != null && await _userRepository.CheckPasswordAsync(user, model.Password))
            {
                var token = GenerateToken(user);
                return token;
            }

            throw new Exception("Invalid username or password.");
        }


        public async Task<string> LogoutAsync(string token)
        {
            await _tokenRevocationService.RevokeTokenAsync(token);
            return "Logout successful.";
        }

    }
}