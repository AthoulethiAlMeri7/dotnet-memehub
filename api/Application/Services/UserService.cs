using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.Application.Dtos.UserDtos;
using api.Application.Services.ServiceContracts;
using api.Infrastructure.Persistence.Repositories;
using api.Domain.Models;
using AutoMapper;

namespace api.Application.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(ApplicationUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }


        public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            var user = _mapper.Map<ApplicationUser>(createUserDto);
            var userWithId= await _userRepository.AddAsync(user,createUserDto.Password);
            return _mapper.Map<UserDto>(userWithDto);
        }

        public async Task<IdentityResult> UpdateUserAsync(Guid id, UpdateUserDto updateUserDto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new Exception("User not found");
            
            _mapper.Map(updateUserDto, user);
            await _userRepository.UpdateAsync(user);
            
            return await _userRepository.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteUserAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new Exception("User not found");
            
            return await _userRepository.DeleteAsync(user);
        }

        public async Task<UserDto> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user == null ? null : _mapper.Map<UserDto>(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }        

        public async Task<UserDto> GetUserByIdWithMemesAsync(Guid id)
        {
            var user = await _userRepository.GetUserByIdWithMemesAsync(id);
            return user == null ? null : _mapper.Map<UserDto>(user);
        }

        public async Task<IdentityResult> AddRoleAsync(Guid id, string role)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new Exception("User not found");
            return await _userRepository.AddRoleAsync(user,role);
        }

        public async Task<IEnumerable<UserDto>> GetUsersByEmailAsync(string email)
        {
            var users = await _userRepository.GetByEmailAsync(email);
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }
    }
}
