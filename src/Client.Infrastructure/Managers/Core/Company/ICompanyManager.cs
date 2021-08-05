using RuS.Application.Features.Companies.Commands.AddEdit;
using RuS.Application.Features.Companies.Queries.GetAllPaged;
using RuS.Application.Features.Companies.Queries.GetById;
using RuS.Application.Requests.Core;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Client.Infrastructure.Managers.Core.Company
{
    public interface ICompanyManager : IManager
    {
        Task<PaginatedResult<GetAllPagedCompaniesResponse>> GetProductsAsync(GetAllPagedCompaniesRequest request);

        Task<IResult<int>> SaveAsync(AddEditCompanyCommand command);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");

        Task<IResult<GetCompanyResponse>> GetByIdAsync(int id);
    }
}
