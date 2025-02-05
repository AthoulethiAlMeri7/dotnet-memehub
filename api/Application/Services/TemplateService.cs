using api.Application.Dtos;
using api.Application.Services.ServiceContracts;
using API.Domain.Interfaces;
using API.Domain.Models;
using AutoMapper;

namespace api.Application.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly ITemplateRepository _templateRepository;
        private readonly IMapper _mapper;
        public TemplateService(ITemplateRepository templateRepository, IMapper mapper)
        {
            _templateRepository = templateRepository;
            _mapper = mapper;
        }

        public async Task<CreateTemplateDto> CreateTemplateAsync(CreateTemplateDto TemplateDto)
        {
            try
            {
                var template = _mapper.Map<Template>(TemplateDto);
                template.Id = Guid.NewGuid();
                var createdTemplate = await _templateRepository.AddAsync(template);
                return _mapper.Map<CreateTemplateDto>(createdTemplate);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteTemplateAsync(Guid id)
        {
            try
            {
                var template = await _templateRepository.GetByIdAsync(id) ?? throw new Exception("Template not found");
                await _templateRepository.DeleteAsync(template);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PagedResult<ApiTemplateDto>> GetAllTemplatesAsync(int pageNumber, int pageSize)
        {
            try
            {
                var (templates, totalCount) = await _templateRepository.GetAllAsync(pageNumber, pageSize);
                return new PagedResult<ApiTemplateDto>
                {
                    Items = _mapper.Map<List<ApiTemplateDto>>(templates),
                    TotalRecords = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiTemplateDto> GetTemplateByIdAsync(Guid id)
        {
            try
            {
                var template = await _templateRepository.GetByIdAsync(id) ?? throw new Exception("Template not found");
                return _mapper.Map<ApiTemplateDto>(template);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> TemplateExists(Guid templateId)
        {
            var template = await _templateRepository.GetByIdAsync(templateId);
            return template != null;
        }

        public async Task<UpdateTemplateDto> UpdateTemplateAsync(Guid id, UpdateTemplateDto templateDto)
        {
            try
            {
                var existingTemplate = await _templateRepository.GetByIdAsync(id);
                if (existingTemplate == null)
                {
                    throw new Exception("Template not found.");
                }
                _mapper.Map(templateDto, existingTemplate);
                var updatedTemplate = await _templateRepository.UpdateAsync(existingTemplate);
                return _mapper.Map<UpdateTemplateDto>(updatedTemplate);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}