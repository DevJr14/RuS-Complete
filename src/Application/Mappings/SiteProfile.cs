using AutoMapper;
using RuS.Application.Features.Sites.Commands.AddEdit;
using RuS.Application.Features.Sites.Queries.GetById;
using RuS.Domain.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Application.Mappings
{
    public class SiteProfile : Profile
    {
        public SiteProfile()
        {
            CreateMap<AddEditSiteCommand, Site>().ReverseMap();
            CreateMap<SiteResponse, Site>().ReverseMap();
        }
    }
}
