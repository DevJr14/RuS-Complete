using RuS.Application.Specifications.Base;
using RuS.Domain.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Application.Specifications.Core
{
    public class SiteFilterSpecification : BaseSpecification<Site>
    {
        public SiteFilterSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = s => s.Name.Contains(searchString) || s.Description.Contains(searchString);
            }
            else
            {
                Criteria = s => true;
            }
        }
    }
}
