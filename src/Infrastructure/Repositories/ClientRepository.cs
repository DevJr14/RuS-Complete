using Microsoft.EntityFrameworkCore;
using RuS.Application.Features.Clients.Commands.AddEdit;
using RuS.Application.Interfaces.Repositories;
using RuS.Domain.Entities.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RuS.Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly IRepositoryAsync<Client, int> _repository;

        public ClientRepository(IRepositoryAsync<Client, int> repository)
        {
            _repository = repository;
        }

        public async Task<bool> IsUniqueEntry(AddEditClientCommand command)
        {
            List<Client> clients = await _repository.Entities.ToListAsync();
            if (command.Id == 0)
            {
                return clients.Any(c => string.Equals(c.Name, command.Name, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                return clients.Any(c => string.Equals(c.Name, command.Name, StringComparison.OrdinalIgnoreCase) && c.Id != command.Id);
            }
        }
    }
}
