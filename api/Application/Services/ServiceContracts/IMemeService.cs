using API.Domain.Models;
using api.Application.Dtos;
namespace api.Application.Services.ServiceContracts
{
    public interface IMemeService
    {
        Task<Meme> CreateMemeAsync(CreateMemeDto createMemeDto);
        Task<Meme> UpdateMemeAsync(Guid id, UpdateMemeDto updateMemeDto);
        Task<Meme> DeleteMemeAsync(Guid id);
        Task<Meme> GetMemeByIdAsync(Guid id);
        Task<IEnumerable<Meme>> GetMemesAsync();
    }
}