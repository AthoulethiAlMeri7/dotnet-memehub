using API.Domain.Models;
using api.Application.Dtos;
namespace api.Application.Services.ServiceContracts
{
    public interface IMemeService
    {
        Task<CreateMemeDto> CreateMemeAsync(CreateMemeDto createMemeDto);
        Task<UpdateMemeDto> UpdateMemeAsync(Guid id, UpdateMemeDto updateMemeDto);
        Task DeleteMemeAsync(Guid id);
        Task<MemeDto> GetMemeByIdAsync(Guid id);
        Task<IEnumerable<MemeDto>> GetAllMemesAsync();
    }
}