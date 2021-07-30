using AutoMapper;
using RuS.Infrastructure.Models.Identity;
using RuS.Application.Responses.Identity;

namespace RuS.Infrastructure.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserResponse, ApplicationUser>().ReverseMap();
            CreateMap<ChatUserResponse, ApplicationUser>().ReverseMap()
                .ForMember(dest => dest.EmailAddress, source => source.MapFrom(source => source.Email)); //Specific Mapping
        }
    }
}