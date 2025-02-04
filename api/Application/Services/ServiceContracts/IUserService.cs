using System;
using System.Threading.Tasks;
using API.Application.Dtos;
using API.Domain.Models;

namespace api.Application.Services.ServiceContracts
{
    public interface IUserService
    {
        Task<UserDto> GetCurrentUserAsync();
    }
}