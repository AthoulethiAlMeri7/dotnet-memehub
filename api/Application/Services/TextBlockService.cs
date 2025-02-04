using api.Application.Dtos;
using api.Application.Services.ServiceContracts;
using API.Domain.Interfaces;
using API.Domain.Models;
using AutoMapper;

namespace api.Application.Services
{
    public class TextBlockService : ITextBlockService
    {
        private readonly ITextBlockRepository _textBlockRepository;
        private readonly IMemeRepository _memeRepository;
        private readonly IMapper _mapper;

        public TextBlockService(ITextBlockRepository textBlockRepository, IMapper mapper, IMemeRepository memeRepository)
        {
            _textBlockRepository = textBlockRepository;
            _mapper = mapper;
            _memeRepository = memeRepository;
        }

        public async Task<TextBlockDto> CreateTextBlockAsync(CreateTextBlockDto textBlockDto)
        {
            try
            {
                var meme = await _memeRepository.GetByIdAsync(textBlockDto.MemeId);
                if (meme == null) throw new Exception("Meme not found.");
                var textBlock = _mapper.Map<TextBlock>(textBlockDto);
                textBlock.Id = Guid.NewGuid();
                var createdTextBlock = await _textBlockRepository.AddAsync(textBlock);
                return _mapper.Map<TextBlockDto>(createdTextBlock);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }

        public async Task DeleteTextBlockAsync(Guid id)
        {
            try
            {
                var textBlock = await _textBlockRepository.GetByIdAsync(id) ?? throw new Exception("TextBlock not found");
                await _textBlockRepository.DeleteAsync(textBlock);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<TextBlockDto>> GetAllTextBlocksAsync()
        {
            try
            {
                var textBlocks = await _textBlockRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<TextBlockDto>>(textBlocks);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<TextBlockDto> GetTextBlockByIdAsync(Guid id)
        {
            try
            {
                var textBlock = await _textBlockRepository.GetByIdAsync(id) ?? throw new Exception("TextBlock not found"); return _mapper.Map<TextBlockDto>(textBlock);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<TextBlockDto>> GetTextBlocksByMemeIdAsync(Guid id)
        {
            var meme = await _memeRepository.GetByIdAsync(id);
            if (meme == null) throw new Exception("Meme not found.");
            var textBlocks = await _textBlockRepository.GetByFilterAsync(t => t.MemeId == id);
            return _mapper.Map<IEnumerable<TextBlockDto>>(textBlocks);
        }

        public async Task<UpdateTextBlockDto> UpdateTextBlockAsync(Guid id, UpdateTextBlockDto textBlockDto)
        {
            try
            {
                var existingTextBlock = await _textBlockRepository.GetByIdAsync(id);
                if (existingTextBlock == null) throw new Exception("TextBlock not found.");
                _mapper.Map(textBlockDto, existingTextBlock);
                var updatedTextBlock = await _textBlockRepository.UpdateAsync(existingTextBlock);
                return _mapper.Map<UpdateTextBlockDto>(updatedTextBlock);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}