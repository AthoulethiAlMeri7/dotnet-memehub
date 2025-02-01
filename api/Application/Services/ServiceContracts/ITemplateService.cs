using api.Application.Dtos;

namespace api.Application.Services.ServiceContracts
{
    public interface ITemplateService
    {
        Task<CreateTemplateDto> CreateTemplateAsync(CreateTemplateDto TemplateDto);
        Task<UpdateTemplateDto> UpdateTemplateAsync(Guid id, UpdateTemplateDto TemplateDto);
        Task DeleteTemplateAsync(Guid id);
        Task<ApiTemplateDto> GetTemplateByIdAsync(Guid id);
        Task<IEnumerable<ApiTemplateDto>> GetAllTemplatesAsync();
    }
}