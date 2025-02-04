using api.Application.Dtos;
using api.Application.Services.ServiceContracts;
using API.Application.Dtos;
using API.Domain.Interfaces;
using API.Domain.Models;
using AutoMapper;

namespace api.Application.Services
{
    public class MemeService : IMemeService
    {
        private readonly IMemeRepository _memeRepository;
        private readonly IApplicationUserRepository _userRepository;
        private readonly IMapper _mapper;
        public MemeService(IMemeRepository memeRepository, IMapper mapper, IApplicationUserRepository userRepository)
        {
            _userRepository = userRepository;
            _memeRepository = memeRepository;
            _mapper = mapper;
        }
        public async Task<MemeDto> CreateMemeAsync(UserDto user, CreateMemeDto createMemeDto)
        {
            try
            {
                var meme = _mapper.Map<Meme>(createMemeDto);
                meme.Id = Guid.NewGuid();
                meme.UserId = user.Id;
                var createdMeme = await _memeRepository.AddAsync(meme);
                return _mapper.Map<MemeDto>(createdMeme);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteMemeAsync(Guid id)
        {
            try
            {
                var meme = await _memeRepository.GetByIdAsync(id) ?? throw new Exception("Meme not found");
                await _memeRepository.DeleteAsync(meme);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<MemeDto>> GetAllMemesAsync()
        {
            try
            {
                var memes = await _memeRepository.GetByFilterAsync(m => !m.IsDeleted);
                return _mapper.Map<IEnumerable<MemeDto>>(memes);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<MemeDto> GetMemeByIdAsync(Guid id)
        {
            try
            {
                var memes = await _memeRepository.GetByFilterAsync(m => m.Id == id && m.IsDeleted == false);
                var meme = memes.FirstOrDefault() ?? throw new Exception("Meme not found"); return _mapper.Map<MemeDto>(meme);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<MemeDto>> GetMemesByUserIdAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new Exception("User not found.");
            var memes = await _memeRepository.GetByUserAsync(userId);
            return _mapper.Map<IEnumerable<MemeDto>>(memes);
        }

        public async Task<MemeDto> UpdateMemeAsync(Guid id, UpdateMemeDto updateMemeDto)
        {
            try
            {
                var existingMemes = await _memeRepository.GetByFilterAsync(m => m.Id == id && m.IsDeleted == false);
                var existingMeme = existingMemes.FirstOrDefault();
                if (existingMeme == null) throw new Exception("Meme not found.");
                _mapper.Map(updateMemeDto, existingMeme);
                var updatedMeme = await _memeRepository.UpdateAsync(existingMeme);
                return _mapper.Map<MemeDto>(updatedMeme);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}