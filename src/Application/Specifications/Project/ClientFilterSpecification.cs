using RuS.Application.Specifications.Base;
using RuS.Domain.Entities.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Application.Specifications.Project
{
    public class ClientFilterSpecification : BaseSpecification<Client>
    {
        public ClientFilterSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = c => c.Name.Contains(searchString) || c.ContactPerson.Contains(searchString)
                    || c.Street.Contains(searchString) || c.Suburb.Contains(searchString)
                    ||c.City.Contains(searchString);
            }
            else
            {
                Criteria = c => true;
            }
        }
    }
}
