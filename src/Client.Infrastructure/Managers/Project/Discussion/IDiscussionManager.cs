using RuS.Application.Features.Discussions.Commands.AddEdit;
using RuS.Application.Features.Discussions.Queries;
using RuS.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RuS.Client.Infrastructure.Managers.Project.Discussion
{
    public interface IDiscussionManager : IManager
    {
        Task<IResult<List<DiscussionResponse>>> GetAllForTaskAsync(int taskId);

        Task<IResult<List<DiscussionResponse>>> GetAllForProjectAsync(int projectId);

        Task<IResult<int>> SaveAsync(AddEditDiscussionCommand command);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<DiscussionResponse>> GetByIdAsync(int id);
    }
}
