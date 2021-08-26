using RuS.Domain.Contracts;
using RuS.Domain.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Domain.Entities.Projects
{
    public class Project : AuditableEntity<int>
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public int SiteId { get; set; }

        public int? StatusId { get; set; }

        public int? CategoryId { get; set; }

        public int? PriorityId { get; set; }

        public int ClientId { get; set; }

        public string Description { get; set; }

        public string ScopeOfWork { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        public DateTime? ActualStart { get; set; }

        public DateTime? ActualEnd { get; set; }       

        public Site Site { get; set; }
        public Status Status { get; set; }
        public Category Category { get; set; }
        public Priority Priority { get; set; }
        public Client Client { get; set; }
        public List<Task> Tasks { get; set; } = new();
    }
}
