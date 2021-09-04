using RuS.Domain.Contracts;
using RuS.Domain.Entities.Core;
using System.Collections.Generic;

namespace RuS.Domain.Entities.Projects
{
    public class Team : AuditableEntity<int>
    {
        public string Name { get; set; }

        public int ProjectId { get; set; }

        public string Description { get; set; }

        public int TeamLeaderId { get; set; }

        public Project Project { get; set; }

        public List<Employee> Members { get; set; } = new();
    }
}
