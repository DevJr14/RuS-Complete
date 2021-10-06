using RuS.Application.Features.Priorities.Commands;
using RuS.Application.Features.Priorities.Queries;
using RuS.Client.Infrastructure.Extensions;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Client.Infrastructure.Managers.Project.Priority
{
    public class PriorityManager : IPriorityManager
    {
        private readonly HttpClient _httpClient;

        public PriorityManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.PriorityEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.PriorityEndpoints.Export
                : Routes.PriorityEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<List<PriorityResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.PriorityEndpoints.GetAll);
            return await response.ToResult<List<PriorityResponse>>();
        }

        public async Task<IResult<PriorityResponse>> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync(Routes.PriorityEndpoints.GetById(id));
            return await response.ToResult<PriorityResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditPriorityCommand command)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.PriorityEndpoints.Save, command);
            return await response.ToResult<int>();
        }
    }
}
