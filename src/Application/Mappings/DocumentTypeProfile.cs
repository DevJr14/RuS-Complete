using AutoMapper;
using RuS.Application.Features.DocumentTypes.Commands.AddEdit;
using RuS.Application.Features.DocumentTypes.Queries.GetAll;
using RuS.Application.Features.DocumentTypes.Queries.GetById;
using RuS.Domain.Entities.Misc;

namespace RuS.Application.Mappings
{
    public class DocumentTypeProfile : Profile
    {
        public DocumentTypeProfile()
        {
            CreateMap<AddEditDocumentTypeCommand, DocumentType>().ReverseMap();
            CreateMap<GetDocumentTypeByIdResponse, DocumentType>().ReverseMap();
            CreateMap<GetAllDocumentTypesResponse, DocumentType>().ReverseMap();
        }
    }
}