using API.Domain.Models;
using AutoMapper;

namespace api.Application.MappingProfiles
{
    public class TextBlockMappingProfile : Profile
    {
        public TextBlockMappingProfile()
        {
            CreateMap<TextBlock, TextBlockDto>();
            CreateMap<CreateTextBlockDto, TextBlock>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Meme, opt => opt.Ignore());
            CreateMap<UpdateTextBlockDto, TextBlock>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Meme, opt => opt.Ignore());

        }
    }
}