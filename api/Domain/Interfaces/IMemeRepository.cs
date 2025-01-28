using API.Domain.Models;

namespace API.Domain.Interfaces
{
    public interface IMemeRepository
    {
        Task<IEnumerable<Meme>> GetAllAsync();
        Task<Meme?> GetByIdAsync(Guid id);
        Task<Meme> AddAsync(Meme entity);
        Task<Meme?> UpdateAsync(Meme entity);
        Task DeleteAsync(Meme entity);
        Task<IEnumerable<Meme>> GetMemesByUser(Guid userId);
        Task<IEnumerable<Meme>> GetMemesByDate(DateTime date);
    }
}