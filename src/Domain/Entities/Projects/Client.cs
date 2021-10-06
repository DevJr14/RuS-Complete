using RuS.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Domain.Entities.Projects
{
    public class Client : AuditableEntity<int>
    {
        public string Name { get; set; }

        public string ContactPerson { get; set; }

        public string TelephoneNo { get; set; }

        public string CellphoneNo { get; set; }

        public string Email { get; set; }

        public string Street { get; set; }

        public string Suburb { get; set; }

        public string City { get; set; }

        public string Postcode { get; set; }

        public List<Project> Projects { get; set; } = new();
    }
}
