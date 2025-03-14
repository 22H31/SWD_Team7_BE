using api.Dtos.Account;
using AutoMapper;
using BE_Team7.Dtos.User;

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
            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.AvatarImages != null && src.AvatarImages.Any() ? src.AvatarImages.First().ImageUrl : null));
            CreateMap<User, UserDetailDto>()
            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.AvatarImages != null && src.AvatarImages.Any() ? src.AvatarImages.First().ImageUrl : null))
            .ForMember(dest => dest.SkinType, opt => opt.MapFrom(src =>
                src.RerultSkinTest
                    .OrderByDescending(test => test.RerultCreateAt) 
                    .Select(test => test.SkinType)
                    .FirstOrDefault()
            ));
           CreateMap<UpdateUserRequestDto, User>()
           .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}