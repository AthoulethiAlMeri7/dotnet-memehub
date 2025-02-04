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
    public class TextBlockRepository : ITextBlockRepository
    {
        private readonly IGenericRepository<TextBlock> _repository;
        private readonly ApplicationDbContext _dbContext;

        public TextBlockRepository(IGenericRepository<TextBlock> repository, ApplicationDbContext dbContext)
        {
            _repository = repository;
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TextBlock>> GetAllAsync()
        {
            return await _dbContext.TextBlocks
            .Include(t => t.Meme)
            .ToListAsync();
        }

        public async Task<TextBlock?> GetByIdAsync(Guid id)
        {
            return await _dbContext.TextBlocks
            .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<TextBlock> AddAsync(TextBlock textBlock)
        {
            return await _repository.AddAsync(textBlock);
        }

        public async Task<TextBlock?> UpdateAsync(TextBlock textBlock)
        {
            return await _repository.UpdateAsync(textBlock);
        }

        public async Task DeleteAsync(TextBlock textBlock)
        {
            await _repository.DeleteAsync(textBlock);
        }



        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _repository.ExistsAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<TextBlock>> GetByFilterAsync(Expression<Func<TextBlock, bool>> filter)
        {
            return await _repository.GetByFilterAsync(filter);
        }

    }
}