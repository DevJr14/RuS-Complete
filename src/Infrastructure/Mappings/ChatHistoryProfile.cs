using AutoMapper;
using RuS.Application.Interfaces.Chat;
using RuS.Application.Models.Chat;
using RuS.Infrastructure.Models.Identity;

namespace RuS.Infrastructure.Mappings
{
    public class ChatHistoryProfile : Profile
    {
        public ChatHistoryProfile()
        {
            CreateMap<ChatHistory<IChatUser>, ChatHistory<ApplicationUser>>().ReverseMap();
        }
    }
}