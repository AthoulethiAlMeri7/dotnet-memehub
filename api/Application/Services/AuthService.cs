using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using api.Application.Dtos.AuthDtos;
using api.Application.Exceptions;
using api.Application.Interfaces;
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
        private readonly IEmailService _emailService;
        private readonly IWebHostEnvironment _env;

        public AuthService(IApplicationUserRepository userRepository, IOptions<JWTBearerTokenSettings> jwtTokenOptions,
            IRevokedTokenService tokenRevocationService,
            IMapper mapper,
            IEmailService emailService,
            IWebHostEnvironment webHostEnvironment)
        {
            _jwtBearerTokenSettings = jwtTokenOptions.Value;
            _userRepository = userRepository;
            _tokenRevocationService = tokenRevocationService;
            _mapper = mapper;
            _emailService = emailService;
            _env = webHostEnvironment;

        }

        public async Task<string> RegisterAsync(RegisterDto userDetails)
        {
            try
            {
                var user = _mapper.Map<ApplicationUser>(userDetails);
                var result = await _userRepository.AddAsync(user, userDetails.Password);

                if (result != null)
                {
                    await _userRepository.AddRoleAsync(user, "ROLE_GUEST");
                    var verificationUrl = $"http://localhost:5145/api/users/verify-email?userId={user.Id}";

                    var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Infrastructure", "Services", "MailingService", "Templates", "VerificationTemplate.html");

                    await _emailService.SendEmailAsync(userDetails.Email, "Email Verification", templatePath, verificationUrl);

                    var token = await LoginAsync(new LoginDto { Username = userDetails.Username, Password = userDetails.Password });

                    if (token != null)
                    {
                        return token;
                    }
                    return "Registered Successfully! Please check your mail to start the Meme War!";
                }
                else
                {
                    throw new RegistrationFailedException("User registration failed.");
                }
            }
            catch
            {
                throw new RegistrationFailedException("An error occurred during registration.");
            }
        }

        private async Task<string> GenerateToken(ApplicationUser applicationUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtBearerTokenSettings.SecretKey);
            var now = DateTime.UtcNow;

            var user = await _userRepository.GetByUserNameAsync(applicationUser.UserName);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            // Fetch user roles
            var roles = (await _userRepository.GetByUserNameAsync(user.UserName)).Roles;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, applicationUser.UserName),
                new Claim(ClaimTypes.Email, applicationUser.Email)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
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
            try
            {

                var user = await _userRepository.GetByUserNameAsync(model.Username);
                if (user == null || !await _userRepository.CheckPasswordAsync(user, model.Password))
                {
                    throw new InvalidCredentialsException("Invalid username or password.");
                }
                var token = await GenerateToken(user);
                return token;
            }
            catch
            {
                throw new InvalidCredentialsException("An error occurred during login.");
            }
        }


        public async Task<string> LogoutAsync(string token)
        {
            try
            {
                await _tokenRevocationService.RevokeTokenAsync(token);
                return "Logout successful.";
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                throw new Exception("An error occurred during logout.", ex);
            }
        }


    }
}