using RuS.Application.Specifications.Base;
using RuS.Domain.Entities.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Application.Specifications.Project
{
    public class TeamFilterSpecification : BaseSpecification<Team>
    {
        public TeamFilterSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = t => t.Name.Contains(searchString) || t.Description.Contains(searchString);
            }
            else
            {
                Criteria = t => true;
            }
        }
    }
}
