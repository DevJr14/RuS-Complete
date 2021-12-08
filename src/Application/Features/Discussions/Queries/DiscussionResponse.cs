using AutoMapper;
using RuS.Application.Features.Projects.Queries;
using RuS.Application.Features.Tasks.Queries;
using RuS.Application.Mappings;
using RuS.Domain.Entities.Projects;

namespace RuS.Application.Features.Discussions.Queries
{
    public class DiscussionResponse : IMapFrom<Discussion>
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public int ProjectId { get; set; }
        public int? TaskId { get; set; }
        public ProjectResponse Project { get; set; } = new();
        public TaskResponse Task { get; set; } = new();
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Discussion, DiscussionResponse>();
        }
    }
}
