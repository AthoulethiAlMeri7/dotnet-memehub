
using System.Security.Claims;
using api.Application.Services.ServiceContracts;
using API.Domain.Interfaces;
using API.Domain.Models;

namespace api.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IApplicationUserRepository _userRepository;

        public UserService(IHttpContextAccessor httpContextAccessor, IApplicationUserRepository userRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
            {
                return null;
            }

            return await _userRepository.GetByIdAsync(userId);
        }
    }
}