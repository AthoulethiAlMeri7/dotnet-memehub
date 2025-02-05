using api.Application.Dtos;
using api.Application.Services.ServiceContracts;
using API.Domain.Interfaces;
using API.Domain.Models;
using AutoMapper;
using api.Application.Dtos.UserDtos;

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
        public async Task<MemeDto> CreateMemeAsync(ReturnedUserDto user, CreateMemeDto createMemeDto)
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

        public async Task<PagedResult<MemeDto>> GetAllMemesAsync(int pageNumber, int pageSize)
        {
            try
            {
                var (memes, totalCount) = await _memeRepository.GetAllAsync(pageNumber, pageSize);

                return new PagedResult<MemeDto>
                {
                    Items = _mapper.Map<List<MemeDto>>(memes),
                    TotalRecords = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<PagedResult<MemeDto>> GetMemesByUserIdAsync(Guid userId, int pageNumber, int pageSize)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new Exception("User not found.");
            var (memes, totalCount) = await _memeRepository.GetByUserAsync(userId, pageNumber, pageSize);
            return new PagedResult<MemeDto>
            {
                Items = _mapper.Map<List<MemeDto>>(memes),
                TotalRecords = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<MemeDto> GetMemeByIdAsync(Guid id)
        {
            try
            {
                var meme = await _memeRepository.GetByIdAsync(id) ?? throw new Exception("Meme not found"); return _mapper.Map<MemeDto>(meme);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<MemeDto> UpdateMemeAsync(Guid id, UpdateMemeDto updateMemeDto)
        {
            try
            {
                var existingMeme = await _memeRepository.GetByIdAsync(id);
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