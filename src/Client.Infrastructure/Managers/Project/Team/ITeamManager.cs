using RuS.Application.Features.Teams.Commands.AddEdit;
using RuS.Application.Features.Teams.Queries;
using RuS.Application.Responses.Project.Team;
using RuS.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RuS.Client.Infrastructure.Managers.Project.Team
{
    public interface ITeamManager : IManager
    {
        Task<IResult<List<TeamResponse>>> GetAllAsync();

        Task<IResult<List<TeamResponse>>> GetAllForProjectAsync(int projectId);

        Task<IResult<TeamMembersResponse>> GetTeamMembers(int teamId);

        Task<IResult<int>> SaveAsync(AddEditTeamCommand command);

        Task<IResult<int>> SaveMemberAsync(AddRemoveTeamMemberCommand command);

        Task<IResult<int>> UpdateTeamMembersAsync(UpdateTeamMembersCommand command);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");

        Task<IResult<TeamResponse>> GetByIdAsync(int id);
    }
}
