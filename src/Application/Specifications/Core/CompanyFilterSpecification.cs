using RuS.Application.Specifications.Base;
using RuS.Domain.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Application.Specifications.Core
{
    public class CompanyFilterSpecification : BaseSpecification<Company>
    {
        public CompanyFilterSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = c => c.Name.Contains(searchString) || c.RegistrationNo.Contains(searchString);
            }
            else
            {
                Criteria = p => true;
            }
        }
    }
}
