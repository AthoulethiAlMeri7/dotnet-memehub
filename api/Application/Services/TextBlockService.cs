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
        private readonly IMapper _mapper;
        public TextBlockService(ITextBlockRepository textBlockRepository, IMapper mapper)
        {
            _textBlockRepository = textBlockRepository;
            _mapper = mapper;
        }


        public async Task<TextBlockDto> CreateTextBlockAsync(CreateTextBlockDto textBlockDto)
        {
            try
            {
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
                var textBlock = await _textBlockRepository.GetByIdAsync(id) ?? throw new Exception("TextBlock not found");
                return _mapper.Map<TextBlockDto>(textBlock);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<TextBlock>> GetTextBlocksByMemeIdAsync(Guid id)
        {
            return await _textBlockRepository.GetByFilterAsync(m => m.MemeId == id);
        }

        public async Task<UpdateTextBlockDto> UpdateTextBlockAsync(Guid id, UpdateTextBlockDto textBlockDto)
        {
            try
            {
                var existingTextBlock = await _textBlockRepository.GetByIdAsync(id);
                if (existingTextBlock == null)
                {
                    throw new Exception("TextBlock not found.");
                }

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