using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using api.Application.Dtos.UserDtos;
using Microsoft.AspNetCore.Mvc.RazorPages;
using api.Application.Dtos;

namespace api.Application.Services.ServiceContracts
{
    public interface IUserService
    {
        Task<ReturnedUserDto?> GetCurrentUserAsync();
        Task<ReturnedUserDto> CreateUserAsync(CreateUserDto createUserDto);
        Task<PagedResult<ReturnedUserDto>> GetAllUsersAsync(int pageNumber, int pageSize);
        Task<IEnumerable<ReturnedUserDto>> GetUsersByEmailAsync(string email);
        Task<PagedResult<ReturnedUserDto>> GetUsersByRoleAsync(string role, int pageNumber, int pageSize);
        Task<PagedResult<ReturnedUserDto>> SearchUsersAsync(string search, int pageNumber, int pageSize);
        Task<ReturnedUserDto?> GetUserByIdAsync(Guid id);
        Task<ReturnedUserDto?> GetUserByUserNameAsync(string userName);
        Task<IdentityResult> UpdateUserAsync(Guid id, UpdateUserDto updateUserDto);
        Task<IdentityResult> DeleteUserAsync(Guid id);
        Task<IdentityResult> AddRoleAsync(Guid id, string role);
        Task<IdentityResult> RemoveRoleAsync(Guid id, string role);
        Task<string> UploadProfilePictureAsync(Guid userId, UploadProfilePictureDto uploadProfilePictureDto);
        Task<IdentityResult> VerifyEmailAsync(Guid userId);
        Task<PagedResult<ReturnedUserDto>> GetAllAdminsAsync(int pageNumber, int pageSize);
        Task<int> GetUsersCountAsync();
        Task<int> GetAdminsCountAsync();
    }
}