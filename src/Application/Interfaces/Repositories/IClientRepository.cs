using RuS.Application.Features.Clients.Commands.AddEdit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Application.Interfaces.Repositories
{
    public interface IClientRepository
    {
        Task<bool> IsUniqueEntry(AddEditClientCommand command);
    }
}
