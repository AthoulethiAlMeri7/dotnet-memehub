using api.Application.Services.ServiceContracts;
using API.Domain.Models;

namespace api.Application.Services
{
    public class MemeService : IMemeService
    {
        public Task<Meme> CreateMemeAsync(CreateMemeDto createMemeDto)
        {
            throw new NotImplementedException();
        }

        public Task<Meme> DeleteMemeAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Meme> GetMemeByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Meme>> GetMemesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Meme> UpdateMemeAsync(Guid id, UpdateMemeDto updateMemeDto)
        {
            throw new NotImplementedException();
        }
    }
}