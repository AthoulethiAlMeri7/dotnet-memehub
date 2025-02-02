using System.Linq.Expressions;
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
        Task<IdentityResult> DeleteAsync(Guid id);
        Task<ApplicationUser?> GetByUserNameAsync(string userName);
        Task<IEnumerable<ApplicationUser>> GetByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task<ApplicationUser?> GetByIdWithMemesAsync(Guid id);
        Task<IEnumerable<ApplicationUser>> GetAllWithMemesAsync();
        Task<ApplicationUser?> GetByAsync(Expression<Func<ApplicationUser, bool>> predicate);
        Task<IEnumerable<ApplicationUser>> GetByFilterAsync(Expression<Func<ApplicationUser, bool>> predicate);
        Task<IdentityResult> AddRoleAsync(ApplicationUser user, string role);

    }
}