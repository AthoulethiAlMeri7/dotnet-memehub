using API.Domain.Models;
using api.Application.Dtos;
using API.Application.Dtos;
namespace api.Application.Services.ServiceContracts
{
    public interface IMemeService
    {
        Task<MemeDto> CreateMemeAsync(UserDto user, CreateMemeDto createMemeDto);
        Task<MemeDto> UpdateMemeAsync(Guid id, UpdateMemeDto updateMemeDto);
        Task DeleteMemeAsync(Guid id);
        Task<MemeDto> GetMemeByIdAsync(Guid id);
        Task<PagedResult<MemeDto>> GetAllMemesAsync(int pageNumber, int pageSize);
        Task<PagedResult<MemeDto>> GetMemesByUserIdAsync(Guid userId, int pageNumber, int pageSize);
    }
}