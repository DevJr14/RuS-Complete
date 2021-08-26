using RuS.Application.Features.Tasks.Commands.AddEdit;
using System.Threading.Tasks;

namespace RuS.Application.Interfaces.Repositories
{
    public interface ITaskRepository
    {
        string GenerateProjectCode();

        Task<bool> IsUniqueEntry(AddEditTaskCommand command);
    }
}
