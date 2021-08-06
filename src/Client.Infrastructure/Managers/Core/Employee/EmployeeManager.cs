using RuS.Application.Features.Employees.Commands.AddEdit;
using RuS.Application.Features.Employees.Queries.GetAllPaged;
using RuS.Application.Features.Employees.Queries.GetById;
using RuS.Application.Requests.Core;
using RuS.Client.Infrastructure.Extensions;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Client.Infrastructure.Managers.Core.Employee
{
    public class EmployeeManager : IEmployeeManager
    {
        private readonly HttpClient _httpClient;

        public EmployeeManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.EmployeeEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.EmployeeEndpoints.Export
                : Routes.EmployeeEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<GetEmployeeResponse>> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync(Routes.EmployeeEndpoints.GetById(id));
            return await response.ToResult<GetEmployeeResponse>();
        }

        public async Task<PaginatedResult<GetAllPagedEmployeesResponse>> GetEmployeesAsync(GetAllPagedEmployeesRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.EmployeeEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllPagedEmployeesResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditEmployeeCommand command)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.EmployeeEndpoints.Save, command);
            return await response.ToResult<int>();
        }
    }
}
