using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Domain.Interfaces;
using api.Domain.Models;
using API.Domain.Interfaces;

namespace api.Infrastructure.Persistence.Repositories
{
    public class RevokedTokenRepository : IRevokedTokenRepository
    {
        private readonly IGenericRepository<RevokedToken> _repository;

        public RevokedTokenRepository(IGenericRepository<RevokedToken> repository)
        {
            _repository = repository;
        }

        public async Task<RevokedToken?> GetRevokedTokenAsync(string token)
        {
            return await _repository.GetByAsync(x => x.Token == token);
        }

        public async Task<RevokedToken> RevokeTokenAsync(RevokedToken revokedToken)
        {
            return await _repository.AddAsync(revokedToken);
        }

    }
}