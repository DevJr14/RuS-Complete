using RuS.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Domain.Entities.Core
{
    public class Site : AuditableEntity<int>
    {
        public string Name { get; set; }
        public int CompanyId { get; set; }
        public string Description { get; set; }
        public Company Company { get; set; }
    }
}
