using AutoMapper;
using RuS.Application.Features.Priorities.Commands;
using RuS.Application.Features.Priorities.Queries;
using RuS.Domain.Entities.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Application.Mappings
{
    public class PriorityProfile : Profile
    {
        public PriorityProfile()
        {
            CreateMap<AddEditPriorityCommand, Priority>();
            CreateMap<Priority, PriorityResponse>();
        }
    }
}
