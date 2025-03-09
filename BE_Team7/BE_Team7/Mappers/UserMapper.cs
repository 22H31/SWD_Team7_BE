using api.Dtos.Account;
using AutoMapper;

namespace BE_Team7.Mappers
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            // Map từ User (Entity) sang LoginResponseDto
            CreateMap<User, LoginResponseDto>()
            .ForMember(dest => dest.IsLogedIn, opt => opt.Ignore())   // Vì bạn sẽ set thủ công
            .ForMember(dest => dest.JwtToken, opt => opt.Ignore())    // Sẽ set thủ công
            .ForMember(dest => dest.Roles, opt => opt.Ignore())
            .ForMember(dest => dest.Avartar, otp =>
            {
                otp.PreCondition(src => src.Avatar != null);
                otp.MapFrom(src => src.Avatar!.ImageUrl);
            });
        }
    }
}
