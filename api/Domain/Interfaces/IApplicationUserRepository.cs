using API.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace API.Domain.Interfaces
{
    public interface IApplicationUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllAsync();
        Task<ApplicationUser?> GetByIdAsync(Guid id);
        Task<IdentityResult> AddAsync(ApplicationUser entity, string password);
        Task<IdentityResult> UpdateAsync(ApplicationUser entity);
        Task<IdentityResult> DeleteAsync(ApplicationUser entity);
        Task<ApplicationUser?> GetByUserNameAsync(string userName);
        Task<IEnumerable<ApplicationUser>> GetByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
    }
}