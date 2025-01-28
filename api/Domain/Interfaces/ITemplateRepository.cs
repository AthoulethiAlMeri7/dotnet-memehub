using API.Domain.Models;

namespace API.Domain.Interfaces
{
    public interface ITemplateRepository
    {
        Task<IEnumerable<Template>> GetAllAsync();
        Task<Template?> GetByIdAsync(Guid id);
        Task<Template> AddAsync(Template entity);
        Task<Template?> UpdateAsync(Template entity);
        Task DeleteAsync(Template entity);
    }
}