using RuS.Application.Features.Projects.Commands.AddEdit;
using RuS.Application.Features.Projects.Queries;
using RuS.Client.Infrastructure.Extensions;
using RuS.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace RuS.Client.Infrastructure.Managers.Project.Project
{
    public class ProjectManager : IProjectManager
    {
        private readonly HttpClient _httpClient;

        public ProjectManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.ProjectEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.ProjectEndpoints.Export
                : Routes.ProjectEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<List<ProjectResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.ProjectEndpoints.GetAll);
            return await response.ToResult<List<ProjectResponse>>();
        }

        public async Task<IResult<ProjectResponse>> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync(Routes.ProjectEndpoints.GetById(id));
            return await response.ToResult<ProjectResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditProjectCommand command)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.ProjectEndpoints.Save, command);
            return await response.ToResult<int>();
        }
    }
}
