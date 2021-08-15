using RuS.Application.Features.Projects.Commands.AddEdit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Application.Interfaces.Repositories
{
    public interface IProjectRepository
    {
        string GenerateProjectCode();
        Task<bool> IsUniqueEntry(AddEditProjectCommand command);
    }
}
