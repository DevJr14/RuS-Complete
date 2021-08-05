using RuS.Application.Specifications.Base;
using RuS.Domain.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Application.Specifications.Core
{
    public class EmployeeFilterSpecification : HeroSpecification<Employee>
    {
        public EmployeeFilterSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = e => e.FirstName.Contains(searchString) || e.LastName.Contains(searchString)
                    || e.EmployeeNo.Contains(searchString);
            }
            else
            {
                Criteria = s => true;
            }
        }
    }
}
