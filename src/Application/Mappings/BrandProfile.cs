using AutoMapper;
using RuS.Application.Features.Brands.Commands.AddEdit;
using RuS.Application.Features.Brands.Queries.GetAll;
using RuS.Application.Features.Brands.Queries.GetById;
using RuS.Domain.Entities.Catalog;

namespace RuS.Application.Mappings
{
    public class BrandProfile : Profile
    {
        public BrandProfile()
        {
            CreateMap<AddEditBrandCommand, Brand>().ReverseMap();
            CreateMap<GetBrandByIdResponse, Brand>().ReverseMap();
            CreateMap<GetAllBrandsResponse, Brand>().ReverseMap();
        }
    }
}