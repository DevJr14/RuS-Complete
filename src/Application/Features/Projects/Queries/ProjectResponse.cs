using RuS.Application.Features.Categories.Queries;
using RuS.Application.Features.Clients.Queries;
using RuS.Application.Features.Priorities.Queries;
using RuS.Application.Features.Sites.Queries.GetById;
using RuS.Application.Features.Statuses.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Application.Features.Projects.Queries
{
    public class ProjectResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SiteId { get; set; }
        public int? StatusId { get; set; }
        public int? CategoryId { get; set; }
        public int? PriorityId { get; set; }
        public int ClientId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string ScopeOfWork { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public DateTime? ActualStart { get; set; }
        public DateTime? ActualEnd { get; set; }

        public SiteResponse Site { get; set; }
        public StatusResponse Status { get; set; }
        public CategoryResponse Category { get; set; }
        public PriorityResponse Priority { get; set; }
        public ClientResponse Client { get; set; }
    }
}
