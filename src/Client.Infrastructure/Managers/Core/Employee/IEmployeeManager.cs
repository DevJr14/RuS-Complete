using RuS.Application.Features.Employees.Commands.AddEdit;
using RuS.Application.Features.Employees.Queries.GetAllPaged;
using RuS.Application.Features.Employees.Queries.GetById;
using RuS.Application.Requests.Core;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Client.Infrastructure.Managers.Core.Employee
{
    public interface IEmployeeManager : IManager
    {
        Task<PaginatedResult<GetAllPagedEmployeesResponse>> GetEmployeesAsync(GetAllPagedEmployeesRequest request);

        Task<IResult<int>> SaveAsync(AddEditEmployeeCommand command);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");

        Task<IResult<GetEmployeeResponse>> GetByIdAsync(int id);
    }
}
