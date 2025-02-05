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
using System.Security.Claims;
using api.Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace api.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IApplicationUserRepository _userRepository;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemeService _memeService;
        private readonly IEmailService _emailService;

        public UserService(IApplicationUserRepository userRepository, IMapper mapper, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, IEmailService emailService, IMemeService memeService)
        {
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _mapper = mapper;
            _env = webHostEnvironment;
            _memeService = memeService;
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
            await _userRepository.AddRoleAsync(userWithId, "ROLE_GUEST");
            await SendVerificationEmailAsync(userWithId);
            return _mapper.Map<ReturnedUserDto>(userWithId);
        }

        public async Task<ReturnedUserDto?> GetCurrentUserAsync()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
            {
                return null;
            }

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return null;

            var response = new ReturnedUserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                ProfilePic = user.ProfilePic,
                Roles = user.Roles
            };
            return response;
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

            if (updateUserDto.ProfilePicture != null)
            {
                user.ProfilePic = await UploadFileAsync(updateUserDto.ProfilePicture);
            }

            _mapper.Map(updateUserDto, user);
            return await _userRepository.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteUserAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            // Get all memes by user
            var memes = await _memeService.GetMemesByUserIdAsync(userId, 1, int.MaxValue);

            // Delete all memes
            foreach (var meme in memes.Items)
            {
                await _memeService.DeleteMemeAsync(meme.Id);
            }

            // Delete the user
            var result = await _userRepository.DeleteAsync(userId);

            return result;
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
            var updatedUser = await _userRepository.UpdateAsync(user);

            return user.ProfilePic;
        }
        public async Task<IEnumerable<ReturnedUserDto>> GetAllAdminsAsync()
        {
            var admins = await _userRepository.GetAllAdminsAsync();
            return _mapper.Map<IEnumerable<ReturnedUserDto>>(admins);
        }
        public async Task<int> GetUsersCountAsync()
        {
            return await _userRepository.GetUsersCountAsync();
        }
        public async Task<int> GetAdminsCountAsync()
        {
            return await _userRepository.GetAdminsCountAsync();
        }


        private async Task SendVerificationEmailAsync(ApplicationUser user)
        {
            if (user == null) throw new Exception("User not found");
            var verificationUrl = $"http://localhost:5145/api/users/verify-email?userId={user.Id}";
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Infrastructure", "Services", "MailingService", "Templates", "VerificationTemplate.html");
            await _emailService.SendEmailAsync(user.Email, "Email Verification", templatePath, verificationUrl);
        }

        public async Task<IdentityResult> VerifyEmailAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new Exception("User not found");

            user.EmailConfirmed = true;
            user.EmailConfirmed = true;
            var result = await _userRepository.UpdateAsync(user);
            if (result.Succeeded)
            {
                return await _userRepository.AddRoleAsync(user, "ROLE_USER");
            }
            else return result;
        }

    }
}
