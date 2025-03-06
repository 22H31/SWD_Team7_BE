using AutoMapper;
using BE_Team7.Dtos.Brand;
using BE_Team7.Dtos.FeedBack;
using BE_Team7.Models;

namespace BE_Team7.Mappers
{
    public class FeedbackMapper
    {
        public class BrandMapper : Profile
        {
            public BrandMapper()
            {
                CreateMap<Feedback, FeedbackDto>().ReverseMap();
                // create map
                CreateMap<Feedback, CreateFeebackRequestDto>().ReverseMap();
            }
        }
    }
}
