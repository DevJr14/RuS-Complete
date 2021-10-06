using RuS.Application.Features.Projects.Commands.AddEdit;
using RuS.Application.Features.Projects.Queries;
using RuS.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RuS.Client.Infrastructure.Managers.Project.Project
{
    public interface IProjectManager : IManager
    {
        Task<IResult<List<ProjectResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditProjectCommand command);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");

        Task<IResult<ProjectResponse>> GetByIdAsync(int id);
    }
}
