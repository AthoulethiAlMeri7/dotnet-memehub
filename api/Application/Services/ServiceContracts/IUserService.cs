using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Application.Dtos.UserDtos;

namespace api.Application.Services.ServiceContracts
{
    public interface IUserService
    {
        Task<CreateUserDto> CreateUserAsync(CreateUserDto createUserDto);
        Task<UpdateUserDto> UpdateUserAsync(Guid id, UpdateUserDto updateUserDto);
        Task DeleteUserAsync(Guid id);
        Task<UserDto> GetUserByIdAsync(Guid id);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdWithMemesAsync(Guid id);
        Task AddRoleAsync(Guid id, string role);
        Task<IEnumerable<UserDto>> GetUsersByEmailAsync(string email);

    }
}