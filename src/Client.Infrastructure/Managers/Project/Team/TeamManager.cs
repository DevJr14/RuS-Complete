using RuS.Application.Features.Teams.Commands.AddEdit;
using RuS.Application.Features.Teams.Queries;
using RuS.Application.Responses.Project.Team;
using RuS.Client.Infrastructure.Extensions;
using RuS.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace RuS.Client.Infrastructure.Managers.Project.Team
{
    public class TeamManager : ITeamManager
    {
        private readonly HttpClient _httpClient;

        public TeamManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.TeamEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.TeamEndpoints.Export
                : Routes.TeamEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<List<TeamResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.TeamEndpoints.GetAll);
            return await response.ToResult<List<TeamResponse>>();
        }

        public async Task<IResult<List<TeamResponse>>> GetAllForProjectAsync(int projectId)
        {
            var response = await _httpClient.GetAsync(Routes.TeamEndpoints.GetAllForProject(projectId));
            return await response.ToResult<List<TeamResponse>>();
        }

        public async Task<IResult<TeamResponse>> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync(Routes.TeamEndpoints.GetById(id));
            return await response.ToResult<TeamResponse>();
        }

        public async Task<IResult<TeamMembersResponse>> GetTeamMembers(int teamId)
        {
            var response = await _httpClient.GetAsync(Routes.TeamEndpoints.GetTeamMembers(teamId));
            return await response.ToResult<TeamMembersResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditTeamCommand command)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.TeamEndpoints.Save, command);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> SaveMemberAsync(AddRemoveTeamMemberCommand command)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.TeamEndpoints.SaveMembers, command);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> UpdateTeamMembersAsync(UpdateTeamMembersCommand command)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.TeamEndpoints.UpdateTeamMembers, command);
            return await response.ToResult<int>();
        }
    }
}
