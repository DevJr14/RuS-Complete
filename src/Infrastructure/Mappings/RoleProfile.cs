using AutoMapper;
using RuS.Infrastructure.Models.Identity;
using RuS.Application.Responses.Identity;

namespace RuS.Infrastructure.Mappings
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleResponse, ApplicationRole>().ReverseMap();
        }
    }
}