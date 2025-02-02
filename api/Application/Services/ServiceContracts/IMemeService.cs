using API.Domain.Models;
using api.Application.Dtos;
using API.Application.Dtos;
namespace api.Application.Services.ServiceContracts
{
    public interface IMemeService
    {
        Task<CreateMemeDto> CreateMemeAsync(UserDto user, CreateMemeDto createMemeDto);
        Task<UpdateMemeDto> UpdateMemeAsync(Guid id, UpdateMemeDto updateMemeDto);
        Task DeleteMemeAsync(Guid id);
        Task<MemeDto> GetMemeByIdAsync(Guid id);
        Task<IEnumerable<MemeDto>> GetAllMemesAsync();
    }
}