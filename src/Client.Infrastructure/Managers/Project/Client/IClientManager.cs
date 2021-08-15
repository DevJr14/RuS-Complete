using RuS.Application.Features.Clients.Commands.AddEdit;
using RuS.Application.Features.Clients.Queries;
using RuS.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RuS.Client.Infrastructure.Managers.Project.Client
{
    public interface IClientManager : IManager
    {
        Task<IResult<List<ClientResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditClientCommand command);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");

        Task<IResult<ClientResponse>> GetByIdAsync(int id);
    }
}
