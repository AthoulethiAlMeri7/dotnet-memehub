using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Application.Dtos.AuthDtos;

namespace api.Application.Services.ServiceContracts
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto userDetails);
        Task<string> LoginAsync(LoginDto model);
        Task<string> LogoutAsync(string token);

    }
}