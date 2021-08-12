using RuS.Application.Features.Statuses.Commands;
using RuS.Application.Features.Statuses.Queries;
using RuS.Client.Infrastructure.Extensions;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Client.Infrastructure.Managers.Project.Status
{
    public class StatusManager : IStatusManager
    {
        private readonly HttpClient _httpClient;

        public StatusManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.StatusEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.StatusEndpoints.Export
                : Routes.StatusEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<List<StatusResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.StatusEndpoints.GetAll);
            return await response.ToResult<List<StatusResponse>>();
        }

        public async Task<IResult<StatusResponse>> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync(Routes.StatusEndpoints.GetById(id));
            return await response.ToResult<StatusResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditStatusCommand command)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.StatusEndpoints.Save, command);
            return await response.ToResult<int>();
        }
    }
}
