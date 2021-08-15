using RuS.Application.Features.Clients.Commands.AddEdit;
using RuS.Application.Features.Clients.Queries;
using RuS.Client.Infrastructure.Extensions;
using RuS.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace RuS.Client.Infrastructure.Managers.Project.Client
{
    public class ClientManager : IClientManager
    {
        private readonly HttpClient _httpClient;

        public ClientManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.ClientEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.ClientEndpoints.Export
                : Routes.ClientEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<List<ClientResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.ClientEndpoints.GetAll);
            return await response.ToResult<List<ClientResponse>>();
        }

        public async Task<IResult<ClientResponse>> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync(Routes.ClientEndpoints.GetById(id));
            return await response.ToResult<ClientResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditClientCommand command)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.ClientEndpoints.Save, command);
            return await response.ToResult<int>();
        }
    }
}
