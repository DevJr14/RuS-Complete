using AutoMapper;
using RuS.Application.Features.Categories.Queries;
using RuS.Application.Features.Discussions.Queries;
using RuS.Application.Features.Priorities.Queries;
using RuS.Application.Features.Projects.Queries;
using RuS.Application.Features.Statuses.Queries;
using RuS.Application.Mappings;
using RuS.Domain.Entities.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RuS.Application.Features.Tasks.Queries
{
    public class TaskResponse : IMapFrom<Task>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public int ProjectId { get; set; }

        public int? StatusId { get; set; }

        public int? CategoryId { get; set; }

        public int? PriorityId { get; set; }

        public string Description { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        public ProjectResponse Project { get; set; } = new();
        public StatusResponse Status { get; set; } = new();
        public CategoryResponse Category { get; set; } = new();
        public PriorityResponse Priority { get; set; } = new();
        public List<DiscussionResponse> Discussions { get; set; } = new();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TaskResponse, Task>().ReverseMap();
        }
    }
}
