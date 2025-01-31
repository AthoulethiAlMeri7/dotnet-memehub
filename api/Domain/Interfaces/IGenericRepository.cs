using System.Linq.Expressions;

namespace API.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task<T> AddAsync(T entity);
        Task<T?> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T?> GetByAsync(Expression<Func<T, bool>> predicate);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        public Task<IEnumerable<T>> GetByFilterAsync(Expression<Func<T, bool>> predicate);
    }
}