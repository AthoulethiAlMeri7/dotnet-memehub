using System.Linq.Expressions;
using API.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace API.Domain.Interfaces
{
    public interface IApplicationUserRepository
    {
        Task<(IEnumerable<ApplicationUser> Users, int TotalCount)> GetAllAsync(int pageNumber, int pageSize);
        Task<ApplicationUser?> GetByIdAsync(Guid id);
        Task<ApplicationUser> AddAsync(ApplicationUser entity, string password);
        Task<IdentityResult> UpdateAsync(ApplicationUser entity);
        Task<IdentityResult> DeleteAsync(Guid id);
        Task<ApplicationUser?> GetByUserNameAsync(string userName);
        Task<IEnumerable<ApplicationUser>> GetByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task<ApplicationUser?> GetByAsync(Expression<Func<ApplicationUser, bool>> predicate);
        Task<(IEnumerable<ApplicationUser> Users, int TotalCount)> GetByFilterAsync(Expression<Func<ApplicationUser, bool>> predicate, int pageNumber, int pageSize);
        Task<IdentityResult> AddRoleAsync(ApplicationUser user, string role);
        Task<IdentityResult> RemoveRoleAsync(ApplicationUser user, string role);
        Task<(IEnumerable<ApplicationUser> Users, int TotalCount)> GetByRoleAsync(string role, int pageNumber, int pageSize);
        Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string token);
        Task<int> GetAdminsCountAsync();
        Task SaveChangesAsync();
        Task<(IEnumerable<ApplicationUser> Users, int TotalCount)> GetAllAdminsAsync(int pageNumber, int pageSize);
        Task<int> GetUsersCountAsync();

    }
}