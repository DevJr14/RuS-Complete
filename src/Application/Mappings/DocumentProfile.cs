using AutoMapper;
using RuS.Application.Features.Documents.Commands.AddEdit;
using RuS.Application.Features.Documents.Queries.GetById;
using RuS.Domain.Entities.Misc;

namespace RuS.Application.Mappings
{
    public class DocumentProfile : Profile
    {
        public DocumentProfile()
        {
            CreateMap<AddEditDocumentCommand, Document>().ReverseMap();
            CreateMap<GetDocumentByIdResponse, Document>().ReverseMap();
        }
    }
}