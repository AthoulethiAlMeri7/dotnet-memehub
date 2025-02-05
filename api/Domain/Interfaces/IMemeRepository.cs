using System.Linq.Expressions;
using API.Domain.Models;

namespace API.Domain.Interfaces
{
    public interface IMemeRepository
    {
        Task<(IEnumerable<Meme> Memes, int TotalCount)> GetAllAsync(int pageNumber, int pageSize);
        Task<Meme?> GetByIdAsync(Guid id);
        Task<Meme> AddAsync(Meme entity);
        Task<Meme?> UpdateAsync(Meme entity);
        Task DeleteAsync(Meme entity);
        Task<(IEnumerable<Meme> Memes, int TotalCount)> GetByUserAsync(Guid userId, int pageNumber, int pageSize);
        Task<IEnumerable<Meme>> GetByDateAsync(DateTime date);
        Task<IEnumerable<Meme>> GetByFilterAsync(Expression<Func<Meme, bool>> predicate);

    }
}