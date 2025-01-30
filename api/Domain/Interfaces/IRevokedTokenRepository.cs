using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Domain.Models;

namespace api.Domain.Interfaces
{
    public interface IRevokedTokenRepository
    {
        public Task<RevokedToken> RevokeTokenAsync(RevokedToken revokedToken);
        public Task<RevokedToken?> GetRevokedTokenAsync(string token);
    }
}