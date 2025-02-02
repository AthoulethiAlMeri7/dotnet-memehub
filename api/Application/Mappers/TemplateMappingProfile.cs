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
            CreateMap<Template, ApiTemplateDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.ImageUrl));

        }
    }
}