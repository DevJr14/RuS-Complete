using RuS.Application.Features.Tasks.Commands.AddEdit;
using RuS.Application.Features.Tasks.Queries;
using RuS.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RuS.Client.Infrastructure.Managers.Project.Task
{
    public interface ITaskManager : IManager
    {
        Task<IResult<List<TaskResponse>>> GetAllAsync();

        Task<IResult<List<TaskResponse>>> GetAllForProjectAsync(int projectId);

        Task<IResult<int>> SaveAsync(AddEditTaskCommand command);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");

        Task<IResult<TaskResponse>> GetByIdAsync(int id);
    }
}
