using RuS.Application.Specifications.Base;
using RuS.Domain.Entities.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RuS.Application.Specifications.Project
{
    public class TaskFilterSpecification : BaseSpecification<Task>
    {
        public TaskFilterSpecification(string searchString)
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
