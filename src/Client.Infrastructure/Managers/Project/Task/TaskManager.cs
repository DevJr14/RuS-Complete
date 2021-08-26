using RuS.Application.Features.Tasks.Commands.AddEdit;
using RuS.Application.Features.Tasks.Queries;
using RuS.Client.Infrastructure.Extensions;
using RuS.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace RuS.Client.Infrastructure.Managers.Project.Task
{
    public class TaskManager : ITaskManager
    {
        private readonly HttpClient _httpClient;

        public TaskManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.TaskEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.TaskEndpoints.Export
                : Routes.TaskEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<List<TaskResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.TaskEndpoints.GetAll);
            return await response.ToResult<List<TaskResponse>>();
        }

        public async Task<IResult<List<TaskResponse>>> GetAllForProjectAsync(int projectId)
        {
            var response = await _httpClient.GetAsync(Routes.TaskEndpoints.GetAllForProject(projectId));
            return await response.ToResult<List<TaskResponse>>();
        }

        public async Task<IResult<TaskResponse>> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync(Routes.TaskEndpoints.GetById(id));
            return await response.ToResult<TaskResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditTaskCommand command)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.TaskEndpoints.Save, command);
            return await response.ToResult<int>();
        }
    }
}
