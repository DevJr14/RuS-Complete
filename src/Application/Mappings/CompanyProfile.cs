using AutoMapper;
using RuS.Application.Features.Companies.Commands.AddEdit;
using RuS.Application.Features.Companies.Queries.GetById;
using RuS.Domain.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Application.Mappings
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            CreateMap<AddEditCompanyCommand, Company>().ReverseMap();
            CreateMap<GetCompanyResponse, Company>().ReverseMap();
        }
    }
}
