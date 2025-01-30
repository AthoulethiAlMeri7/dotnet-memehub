using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Application.Services.ServiceContracts;
using api.Domain.Interfaces;
using api.Domain.Models;

namespace api.Application.Services
{
    public class RevokedTokenService : IRevokedTokenService
    {
        private readonly IRevokedTokenRepository _revokedTokenRepository;

        public RevokedTokenService(IRevokedTokenRepository revokedTokenRepository)
        {
            _revokedTokenRepository = revokedTokenRepository;
        }

        public async Task<RevokedToken?> RevokeTokenAsync(string token)
        {
            if (await IsTokenRevokedAsync(token))
            {
                return null;
            }
            var revokedToken = new RevokedToken
            {
                Token = token,
                RevokedAt = DateTime.UtcNow
            };

            return await _revokedTokenRepository.RevokeTokenAsync(revokedToken);
        }

        public async Task<bool> IsTokenRevokedAsync(string token)
        {
            var revokedToken = await _revokedTokenRepository.GetRevokedTokenAsync(token);

            return revokedToken != null;
        }
    }
}