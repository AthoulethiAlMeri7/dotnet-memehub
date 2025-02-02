using api.Application.Dtos;
using API.Domain.Models;
using AutoMapper;

namespace api.Application.MappingProfiles
{
    public class MemeProfile : Profile
    {
        public MemeProfile()
        {
            CreateMap<Meme, MemeDto>()
                .ForMember(dest => dest.TextBlocks, opt => opt.MapFrom(src => src.TextBlocks));

            CreateMap<CreateMemeDto, Meme>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<UpdateMemeDto, Meme>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.TemplateId, opt => opt.Ignore())
                .ForMember(dest => dest.TextBlocks, opt => opt.MapFrom(src => src.TextBlocks));

            CreateMap<Meme, UpdateMemeDto>()
                .ForMember(dest => dest.TextBlocks, opt => opt.MapFrom(src => src.TextBlocks));

        }
    }
}