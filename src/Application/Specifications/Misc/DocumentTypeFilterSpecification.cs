using RuS.Application.Specifications.Base;
using RuS.Domain.Entities.Misc;

namespace RuS.Application.Specifications.Misc
{
    public class DocumentTypeFilterSpecification : BaseSpecification<DocumentType>
    {
        public DocumentTypeFilterSpecification(string searchString)
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