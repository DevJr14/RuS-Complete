using RuS.Application.Specifications.Base;

namespace RuS.Application.Specifications.Project
{
    public class ProjectFilterSpecification : BaseSpecification<Domain.Entities.Projects.Project>
    {
        public ProjectFilterSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.Name.Contains(searchString) || p.Description.Contains(searchString)
                    || p.ScopeOfWork.Contains(searchString) || p.Code.Contains(searchString);
            }
            else
            {
                Criteria = c => true;
            }
        }
    }
}
