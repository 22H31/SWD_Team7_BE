using AutoMapper;
using BE_Team7.Dtos.SkinTestAnswers;
using BE_Team7.Models;

namespace BE_Team7.Mappers
{
    public class SkinTestAnswerMapper : Profile
    {
        public SkinTestAnswerMapper() 
        {
            CreateMap<SkinTestAnswers, SkinTestAnswerDto>().ReverseMap();

            CreateMap<CreateSkinTestAnswersDto, SkinTestAnswers>()
            .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.QuestionId)); // Đảm bảo ánh xạ trường này

            CreateMap<UpdateSkinTestAnswersDto, SkinTestAnswers>()
                .ForMember(dest => dest.AnswerId, opt => opt.Ignore());
        }
    }
}
