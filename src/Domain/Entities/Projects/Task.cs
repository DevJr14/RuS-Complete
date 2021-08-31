using RuS.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Domain.Entities.Projects
{
    public class Task : AuditableEntity<int>
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public int ProjectId { get; set; }

        public int? StatusId { get; set; }

        public int? CategoryId { get; set; }

        public int? PriorityId { get; set; }

        public string Description { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        public Project Project { get; set; }
        public Status Status { get; set; }
        public Category Category { get; set; }
        public Priority Priority { get; set; }
    }
}
