using System;
using System.Threading.Tasks;
using API.Domain.Models;

namespace api.Application.Services.ServiceContracts
{
    public interface IUserService
    {
        Task<ApplicationUser> GetCurrentUserAsync();
    }
}