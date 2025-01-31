using System.Linq.Expressions;
using API.Domain.Models;

namespace API.Domain.Interfaces
{
    public interface ITextBlockRepository
    {
        Task<IEnumerable<TextBlock>> GetAllAsync();
        Task<TextBlock?> GetByIdAsync(Guid id);
        Task<TextBlock> AddAsync(TextBlock entity);
        Task<TextBlock?> UpdateAsync(TextBlock entity);
        Task DeleteAsync(TextBlock entity);
        Task<bool> ExistsAsync(Guid id);
        Task<IEnumerable<TextBlock>> GetByFilterAsync(Expression<Func<TextBlock, bool>> filter);
    }
}