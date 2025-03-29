using AutoMapper;
using BE_Team7.Dtos.SkinTestQuestion;
using BE_Team7.Models;

namespace BE_Team7.Mappers
{
    public class SkinTestQuestionMapper : Profile
    {
        public SkinTestQuestionMapper()
        {
            CreateMap<SkinTestQuestion, SkinTestQuestionDto>().ReverseMap();

            CreateMap<SkinTestQuestion, CreateSkinTestQuestionDto>().ReverseMap();  

            CreateMap<UpdateSkinTestQuestionDto, SkinTestQuestion>()
            .ForMember(dest => dest.QuestionId, opt => opt.Ignore());
        }
    }
}
