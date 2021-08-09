using RuS.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Domain.Entities.Projects
{
    public class Priority : AuditableEntity<int>
    {
        public int Value { get; set; }

        public string Description { get; set; }
    }
}
