using RuS.Application.Specifications.Base;
using RuS.Domain.Entities.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Application.Specifications.Project
{
    public class StatusFilterSpecification : BaseSpecification<Status>
    {
        public StatusFilterSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = c => c.Name.Contains(searchString) || c.Description.Contains(searchString);
            }
            else
            {
                Criteria = c => true;
            }
        }
    }
}
