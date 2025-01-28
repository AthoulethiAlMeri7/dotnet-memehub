using API.Domain.Models;

namespace API.Domain.Interfaces
{
    public interface IApplicationUser
    {
        Task<IEnumerable<ApplicationUser>> GetAllAsync();
        Task<ApplicationUser?> GetByIdAsync(Guid id);
        Task<ApplicationUser> AddAsync(ApplicationUser entity);
        Task<ApplicationUser?> UpdateAsync(ApplicationUser entity);
        Task DeleteAsync(ApplicationUser entity);
        Task<IEnumerable<ApplicationUser>> GetUserByUserName(string userName);
        Task<IEnumerable<ApplicationUser>> GetUserByEmail(string email);
    }
}