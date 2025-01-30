using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Domain.Models;

namespace api.Application.Services.ServiceContracts
{
    public interface IRevokedTokenService
    {
        public Task<RevokedToken?> RevokeTokenAsync(string token);
        public Task<bool> IsTokenRevokedAsync(string token);

    }
}