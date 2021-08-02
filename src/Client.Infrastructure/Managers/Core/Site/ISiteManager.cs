using RuS.Application.Features.Sites.Commands.AddEdit;
using RuS.Application.Features.Sites.Queries.GetAllPaged;
using RuS.Application.Features.Sites.Queries.GetById;
using RuS.Application.Requests.Core;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Client.Infrastructure.Managers.Core.Site
{
    public interface ISiteManager : IManager
    {
        Task<PaginatedResult<GetAllPagedSitesResponse>> GetProductsAsync(GetAllPagedSitesRequest request);

        Task<IResult<int>> SaveAsync(AddEditSiteCommand command);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");

        Task<IResult<GetSiteResponse>> GetByIdAsync(int id);
    }
}
