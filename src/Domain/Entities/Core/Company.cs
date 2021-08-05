using RuS.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Domain.Entities.Core
{
    public class Company : AuditableEntity<int>
    {
        public string Name { get; set; }

        public string ShortTitle { get; set; }

        public string RegistrationNo { get; set; }

        public DateTime? RegistrationDate { get; set; }

        public List<Site> Sites { get; set; } = new();
    }
}
