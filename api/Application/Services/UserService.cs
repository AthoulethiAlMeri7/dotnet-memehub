using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.Application.Dtos.UserDtos;
using api.Application.Services.ServiceContracts;
using api.Infrastructure.Persistence.Repositories;
using api.Domain.Models;
using AutoMapper;
using API.Domain.Models;
using Microsoft.AspNetCore.Identity;
using API.Domain.Interfaces;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace api.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IApplicationUserRepository _userRepository;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public UserService(IApplicationUserRepository userRepository, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _env = webHostEnvironment;
        }

        private async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new Exception("File is empty");

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"/uploads/{uniqueFileName}";
        }


        public async Task<ReturnedUserDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            var user = _mapper.Map<ApplicationUser>(createUserDto);
            if (createUserDto.ProfilePicture != null)
            {
                user.ProfilePic = await UploadFileAsync(createUserDto.ProfilePicture);
            }
            var userWithId = await _userRepository.AddAsync(user, createUserDto.Password);
            await _userRepository.AddRoleAsync(userWithId, "ROLE_USER");
            return _mapper.Map<ReturnedUserDto>(userWithId);
        }

        public async Task<IEnumerable<ReturnedUserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ReturnedUserDto>>(users);
        }

        public async Task<IEnumerable<ReturnedUserDto>> GetUsersByEmailAsync(string email)
        {
            var users = await _userRepository.GetByEmailAsync(email);
            return _mapper.Map<IEnumerable<ReturnedUserDto>>(users);
        }

        public async Task<IEnumerable<ReturnedUserDto>> SearchUsersAsync(string search)
        {
            var users = await _userRepository.GetByFilterAsync(u => u.UserName.Contains(search) || u.Email.Contains(search));
            return _mapper.Map<IEnumerable<ReturnedUserDto>>(users);
        }

        public async Task<IEnumerable<ReturnedUserDto>> GetUsersByRoleAsync(string role)
        {
            var users = await _userRepository.GetByRoleAsync(role);
            return _mapper.Map<IEnumerable<ReturnedUserDto>>(users);
        }

        public async Task<ReturnedUserDto?> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user == null ? null : _mapper.Map<ReturnedUserDto>(user);
        }

        public async Task<ReturnedUserDto?> GetUserByUserNameAsync(string userName)
        {
            var user = await _userRepository.GetByUserNameAsync(userName);
            if (user == null) return null;
            return _mapper.Map<ReturnedUserDto>(user);
        }

        public async Task<IdentityResult> UpdateUserAsync(Guid id, UpdateUserDto updateUserDto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new Exception("User not found");

            if (string.IsNullOrWhiteSpace(updateUserDto.UserName))
            {
                updateUserDto.UserName = user.UserName;
            }

            if (string.IsNullOrWhiteSpace(updateUserDto.Email))
            {
                updateUserDto.Email = user.Email;
            }

            // if (!string.IsNullOrWhiteSpace(updateUserDto.Password))
            // {
            //     var passwordHasher = new PasswordHasher<ApplicationUser>();
            //     user.PasswordHash = passwordHasher.HashPassword(user, updateUserDto.Password);
            // }

            _mapper.Map(updateUserDto, user);
            await _userRepository.UpdateAsync(user);

            return await _userRepository.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteUserAsync(Guid id)
        {
            return await _userRepository.DeleteAsync(id);
        }

        public async Task<IdentityResult> AddRoleAsync(Guid id, string role)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new Exception("User not found");
            return await _userRepository.AddRoleAsync(user, role);
        }

        public async Task<IdentityResult> RemoveRoleAsync(Guid id, string role)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new Exception("User not found");
            return await _userRepository.RemoveRoleAsync(user, role);
        }


        public async Task<string> UploadProfilePictureAsync(Guid userId, UploadProfilePictureDto ppDto)
        {
            if (ppDto.ProfilePicture == null) throw new Exception("File is empty");

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new Exception("User not found");

            user.ProfilePic = await UploadFileAsync(ppDto.ProfilePicture);
            await _userRepository.UpdateAsync(user);

            return user.ProfilePic;
        }
        

    }
}
