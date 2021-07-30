using RuS.Application.Features.Documents.Commands.AddEdit;
using RuS.Application.Features.Documents.Queries.GetAll;
using RuS.Application.Requests.Documents;
using RuS.Shared.Wrapper;
using System.Threading.Tasks;
using RuS.Application.Features.Documents.Queries.GetById;

namespace RuS.Client.Infrastructure.Managers.Misc.Document
{
    public interface IDocumentManager : IManager
    {
        Task<PaginatedResult<GetAllDocumentsResponse>> GetAllAsync(GetAllPagedDocumentsRequest request);

        Task<IResult<GetDocumentByIdResponse>> GetByIdAsync(GetDocumentByIdQuery request);

        Task<IResult<int>> SaveAsync(AddEditDocumentCommand request);

        Task<IResult<int>> DeleteAsync(int id);
    }
}