using api.Application.Dtos;
using API.Domain.Models;

namespace api.Application.Services.ServiceContracts
{
    public interface ITextBlockService
    {
        Task<TextBlockDto> CreateTextBlockAsync(CreateTextBlockDto textBlockDto);
        Task<UpdateTextBlockDto> UpdateTextBlockAsync(Guid id, UpdateTextBlockDto textBlockDto);
        Task DeleteTextBlockAsync(Guid id);
        Task<TextBlockDto> GetTextBlockByIdAsync(Guid id);
        Task<IEnumerable<TextBlockDto>> GetAllTextBlocksAsync();
        Task<IEnumerable<TextBlock>> GetTextBlocksByMemeIdAsync(Guid id);
    }
}