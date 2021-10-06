using AutoMapper;
using RuS.Application.Features.Employees.Queries.GetById;
using RuS.Application.Features.Projects.Queries;
using RuS.Application.Mappings;
using RuS.Domain.Entities.Projects;
using System.Collections.Generic;

namespace RuS.Application.Features.Teams.Queries
{
    public class TeamResponse : IMapFrom<Team>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int ProjectId { get; set; }

        public string Description { get; set; }

        public int TeamLeaderId { get; set; }

        public ProjectResponse Project { get; set; }

        public List<GetEmployeeResponse> Members { get; set; } = new();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Team, TeamResponse>().ReverseMap();
        }
    }
}
