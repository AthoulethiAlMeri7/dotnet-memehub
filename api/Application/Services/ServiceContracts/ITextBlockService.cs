namespace api.Application.Services.ServiceContracts
{
    public interface ITextBlockService
    {
        Task<CreateTextBlockDto> CreateTextBlockAsync(CreateTextBlockDto textBlockDto);
        Task<UpdateTextBlockDto> UpdateTextBlockAsync(Guid id, UpdateTextBlockDto textBlockDto);
        Task DeleteTextBlockAsync(Guid id);
        Task<TextBlockDto> GetTextBlockByIdAsync(Guid id);
        Task<IEnumerable<TextBlockDto>> GetAllTextBlocksAsync();
    }
}