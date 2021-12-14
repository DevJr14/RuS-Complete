using RuS.Application.Features.Clients.Commands.AddEdit;
using System.Threading.Tasks;

namespace RuS.Application.Interfaces.Repositories
{
    public interface IClientRepository
    {
        Task<bool> IsUniqueEntry(AddEditClientCommand command);
        Task<bool> IsInUse(int clientId);
    }
}
