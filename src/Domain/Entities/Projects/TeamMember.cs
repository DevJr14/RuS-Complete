using RuS.Domain.Contracts;
using RuS.Domain.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Domain.Entities.Projects
{
    public class TeamMember : AuditableEntity<int>
    {
        public int TeamId { get; set; }

        public int EmployeeId { get; set; }

        public virtual Employee Member { get; set; }

        public virtual Team Team { get; set; }

    }
}
