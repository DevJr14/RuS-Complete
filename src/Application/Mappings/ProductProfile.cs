using AutoMapper;
using RuS.Application.Features.Products.Commands.AddEdit;
using RuS.Domain.Entities.Catalog;

namespace RuS.Application.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<AddEditProductCommand, Product>().ReverseMap();
        }
    }
}