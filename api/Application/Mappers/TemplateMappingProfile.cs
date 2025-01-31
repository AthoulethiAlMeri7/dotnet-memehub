using api.Application.Dtos;
using API.Domain.Models;
using AutoMapper;

namespace api.Application.MappingProfiles
{
    public class TemplateMappingProfile : Profile
    {
        public TemplateMappingProfile()
        {
            CreateMap<ApiTemplateDto, Template>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Url))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name));
            CreateMap<Template, ApiTemplateDto>();
        }
    }
}