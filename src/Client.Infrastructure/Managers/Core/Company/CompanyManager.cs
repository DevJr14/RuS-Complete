using RuS.Application.Features.Companies.Commands.AddEdit;
using RuS.Application.Features.Companies.Queries.GetAllPaged;
using RuS.Application.Features.Companies.Queries.GetById;
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

namespace RuS.Client.Infrastructure.Managers.Core.Company
{
    public class CompanyManager : ICompanyManager
    {
        private readonly HttpClient _httpClient;

        public CompanyManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PaginatedResult<GetAllPagedCompaniesResponse>> GetCompaniesAsync(GetAllPagedCompaniesRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.CompanyEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllPagedCompaniesResponse>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.CompanyEndpoints.Delete}/{id}" );
            return await response.ToResult<int>();
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.CompanyEndpoints.Export
                : Routes.CompanyEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditCompanyCommand command)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.CompanyEndpoints.Save, command);
            return await response.ToResult<int>();
        }

        public async Task<IResult<GetCompanyResponse>> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync(Routes.CompanyEndpoints.GetById(id));
            return await response.ToResult<GetCompanyResponse>();
        }
    }
}
