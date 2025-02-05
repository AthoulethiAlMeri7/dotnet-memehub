using API.Domain.Models;
using api.Application.Dtos;
using api.Application.Dtos.UserDtos;
namespace api.Application.Services.ServiceContracts
{
    public interface IMemeService
    {
        Task<MemeDto> CreateMemeAsync(ReturnedUserDto user, CreateMemeDto createMemeDto);
        Task<MemeDto> UpdateMemeAsync(Guid id, UpdateMemeDto updateMemeDto);
        Task DeleteMemeAsync(Guid id);
        Task<MemeDto> GetMemeByIdAsync(Guid id);
        Task<PagedResult<MemeDto>> GetAllMemesAsync(int pageNumber, int pageSize);
        Task<PagedResult<MemeDto>> GetMemesByUserIdAsync(Guid userId, int pageNumber, int pageSize);
    }
}