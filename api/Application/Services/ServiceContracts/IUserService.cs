using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using api.Application.Dtos.UserDtos;

namespace api.Application.Services.ServiceContracts
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
        Task<IdentityResult> UpdateUserAsync(Guid id, UpdateUserDto updateUserDto);
        Task <IdentityResult> DeleteUserAsync(Guid id);
        Task<UserDto> GetUserByIdAsync(Guid id);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdWithMemesAsync(Guid id);
        Task<IdentityResult> AddRoleAsync(Guid id, string role);
        Task<IEnumerable<UserDto>> GetUsersByEmailAsync(string email);

    }
}