using RuS.Application.Features.Teams.Commands.AddEdit;
using System.Threading.Tasks;

namespace RuS.Application.Interfaces.Repositories
{
    public interface ITeamRepository
    {
        Task<bool> IsUniqueEntry(AddEditTeamCommand command);
    }
}
