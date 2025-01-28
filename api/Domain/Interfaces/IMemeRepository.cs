using API.Domain.Models;

namespace API.Domain.Interfaces
{
    public interface IMemeRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<IEnumerable<T>> GetMemesByUser(Guid userId);
        Task<IEnumerable<T>> GetMemesByDate(DateTime date);
    }
}