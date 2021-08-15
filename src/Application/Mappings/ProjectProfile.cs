using AutoMapper;
using RuS.Application.Features.Projects.Commands.AddEdit;
using RuS.Application.Features.Projects.Queries;
using RuS.Domain.Entities.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Application.Mappings
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<AddEditProjectCommand, Project>();
            CreateMap<Project, ProjectResponse>();
        }
    }
}
