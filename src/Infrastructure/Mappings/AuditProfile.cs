using AutoMapper;
using RuS.Infrastructure.Models.Audit;
using RuS.Application.Responses.Audit;

namespace RuS.Infrastructure.Mappings
{
    public class AuditProfile : Profile
    {
        public AuditProfile()
        {
            CreateMap<AuditResponse, Audit>().ReverseMap();
        }
    }
}