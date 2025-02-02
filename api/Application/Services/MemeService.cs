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
        private readonly IMapper _mapper;
        public MemeService(IMemeRepository memeRepository, IMapper mapper)
        {
            _memeRepository = memeRepository;
            _mapper = mapper;
        }
        public async Task<CreateMemeDto> CreateMemeAsync(UserDto user, CreateMemeDto createMemeDto)
        {
            try
            {
                var meme = _mapper.Map<Meme>(createMemeDto);
                meme.Id = Guid.NewGuid();
                meme.UserId = user.Id;
                var createdMeme = await _memeRepository.AddAsync(meme);
                return _mapper.Map<CreateMemeDto>(createdMeme);
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

        public async Task<MemeDto> GetMemeByIdAsync(Guid id)
        {
            try
            {
                var meme = await _memeRepository.GetByIdAsync(id) ?? throw new Exception("Meme not found");
                return _mapper.Map<MemeDto>(meme);
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
                var memes = await _memeRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<MemeDto>>(memes);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UpdateMemeDto> UpdateMemeAsync(Guid id, UpdateMemeDto updateMemeDto)
        {
            try
            {
                var existingMeme = await _memeRepository.GetByIdAsync(id);
                if (existingMeme == null)
                {
                    throw new Exception("Meme not found.");
                }
                _mapper.Map(updateMemeDto, existingMeme);
                var updatedMeme = await _memeRepository.UpdateAsync(existingMeme);
                return _mapper.Map<UpdateMemeDto>(updatedMeme);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}