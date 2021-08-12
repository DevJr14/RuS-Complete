using RuS.Application.Specifications.Base;
using RuS.Domain.Entities.Catalog;

namespace RuS.Application.Specifications.Catalog
{
    public class BrandFilterSpecification : BaseSpecification<Brand>
    {
        public BrandFilterSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.Name.Contains(searchString) || p.Description.Contains(searchString);
            }
            else
            {
                Criteria = p => true;
            }
        }
    }
}
