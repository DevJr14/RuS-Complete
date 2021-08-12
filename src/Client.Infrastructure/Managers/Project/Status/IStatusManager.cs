using RuS.Application.Features.Statuses.Commands;
using RuS.Application.Features.Statuses.Queries;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Client.Infrastructure.Managers.Project.Status
{
    public interface IStatusManager : IManager
    {
        Task<IResult<List<StatusResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditStatusCommand command);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");

        Task<IResult<StatusResponse>> GetByIdAsync(int id);
    }
}
