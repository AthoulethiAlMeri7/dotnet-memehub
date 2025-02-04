using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using API.Domain.Interfaces;
using API.Domain.Models;
using API.Infrastructure.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace api.Infrastructure.Persistence.Repositories
{
    public class MemeRepository : IMemeRepository
    {
        private readonly IGenericRepository<Meme> _memeRepository;
        private readonly ApplicationDbContext _dbContext;

        public MemeRepository(IGenericRepository<Meme> memeRepository, ApplicationDbContext dbContext)
        {
            _memeRepository = memeRepository;
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Meme>> GetAllAsync()
        {
            return await _dbContext.Memes
            .Include(m => m.User)
            .Include(m => m.Template)
            .Include(m => m.TextBlocks)
            .ToListAsync();
        }

        public async Task<Meme?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Memes.Include(m => m.User)
            .Include(m => m.Template)
            .Include(m => m.TextBlocks)
            .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Meme> AddAsync(Meme meme)
        {
            return await _memeRepository.AddAsync(meme);
        }

        public async Task<Meme?> UpdateAsync(Meme meme)
        {
            return await _memeRepository.UpdateAsync(meme);
        }

        public async Task DeleteAsync(Meme meme)
        {
            await _memeRepository.DeleteAsync(meme);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _memeRepository.ExistsAsync(m => m.Id == id);
        }

        public async Task<Meme?> GetByAsync(Expression<Func<Meme, bool>> predicate)
        {
            return await _memeRepository.GetByAsync(predicate);
        }

        public async Task<IEnumerable<Meme>> GetByUserAsync(Guid userId)
        {
            return await _dbContext.Memes
            .Include(m => m.User)
            .Include(m => m.Template)
            .Include(m => m.TextBlocks)
            .Where(m => m.UserId == userId)
            .ToListAsync();
        }

        public async Task<IEnumerable<Meme>> GetByDateAsync(DateTime date)
        {
            return await _dbContext.Memes
            .Include(m => m.User)
            .Include(m => m.Template)
            .Include(m => m.TextBlocks)
            .Where(m => m.CreatedAt == date)
            .ToListAsync();
        }

        public async Task<IEnumerable<Meme>> GetByFilterAsync(Expression<Func<Meme, bool>> predicate)
        {
            return await _dbContext.Memes
                .Include(m => m.TextBlocks.Where(t => !t.IsDeleted))
                .Where(predicate)
                .ToListAsync(); ;
        }

    }
}