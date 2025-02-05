using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using api.Application.Dtos.UserDtos;
using API.Application.Dtos;

namespace api.Application.Services.ServiceContracts
{
    public interface IUserService
    {
        Task<UserDto?> GetCurrentUserAsync();
        Task<ReturnedUserDto> CreateUserAsync(CreateUserDto createUserDto);
        Task<IEnumerable<ReturnedUserDto>> GetAllUsersAsync();
        Task<IEnumerable<ReturnedUserDto>> GetUsersByEmailAsync(string email);
        Task<IEnumerable<ReturnedUserDto>> GetUsersByRoleAsync(string role);
        Task<IEnumerable<ReturnedUserDto>> SearchUsersAsync(string search);
        Task<ReturnedUserDto?> GetUserByIdAsync(Guid id);
        Task<ReturnedUserDto?> GetUserByUserNameAsync(string userName);
        Task<IdentityResult> UpdateUserAsync(Guid id, UpdateUserDto updateUserDto);
        Task<IdentityResult> DeleteUserAsync(Guid id);
        Task<IdentityResult> AddRoleAsync(Guid id, string role);
        Task<IdentityResult> RemoveRoleAsync(Guid id, string role);
        Task<string> UploadProfilePictureAsync(Guid userId, UploadProfilePictureDto uploadProfilePictureDto);
        Task<IdentityResult> VerifyEmailAsync(Guid userId);
    }
}