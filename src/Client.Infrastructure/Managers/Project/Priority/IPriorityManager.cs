using RuS.Application.Features.Priorities.Commands;
using RuS.Application.Features.Priorities.Queries;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Client.Infrastructure.Managers.Project.Priority
{
    public interface IPriorityManager : IManager
    {
        Task<IResult<List<PriorityResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditPriorityCommand command);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");

        Task<IResult<PriorityResponse>> GetByIdAsync(int id);
    }
}
