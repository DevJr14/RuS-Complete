using RuS.Application.Features.Discussions.Commands.AddEdit;
using RuS.Application.Features.Discussions.Queries;
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

namespace RuS.Client.Infrastructure.Managers.Project.Discussion
{
    public class DiscussionManager : IDiscussionManager
    {
        private readonly HttpClient _httpClient;

        public DiscussionManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.DiscussionEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<DiscussionResponse>>> GetAllForProjectAsync(int projectId)
        {
            var response = await _httpClient.GetAsync(Routes.DiscussionEndpoints.GetAllForProject(projectId));
            return await response.ToResult<List<DiscussionResponse>>();
        }

        public async Task<IResult<List<DiscussionResponse>>> GetAllForTaskAsync(int taskId)
        {
            var response = await _httpClient.GetAsync(Routes.DiscussionEndpoints.GetAllForTask(taskId));
            return await response.ToResult<List<DiscussionResponse>>();
        }

        public async Task<IResult<DiscussionResponse>> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync(Routes.DiscussionEndpoints.GetById(id));
            return await response.ToResult<DiscussionResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditDiscussionCommand command)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.DiscussionEndpoints.Save, command);
            return await response.ToResult<int>();
        }
    }
}
