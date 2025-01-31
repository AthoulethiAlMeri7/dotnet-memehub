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
    public class TemplateRepository : ITemplateRepository
    {
        private readonly IGenericRepository<Template> _repository;
        private readonly ApplicationDbContext _dbContext;

        public TemplateRepository(IGenericRepository<Template> repository, ApplicationDbContext dbContext)
        {
            _repository = repository;
            _dbContext = dbContext;
        }


        public async Task<IEnumerable<Template>> GetAllAsync()
        {
            return await _dbContext.Templates.Include(t => t.Memes).ToListAsync();
        }

        public async Task<Template?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Templates.Include(t => t.Memes).FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Template> AddAsync(Template template)
        {
            return await _repository.AddAsync(template);
        }

        public async Task<Template?> UpdateAsync(Template template)
        {
            return await _repository.UpdateAsync(template);
        }

        public async Task DeleteAsync(Template template)
        {
            await _repository.DeleteAsync(template);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _repository.ExistsAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Template>> GetByFilterAsync(Expression<Func<Template, bool>> filter)
        {
            return await _repository.GetByFilterAsync(filter);
        }

    }
}