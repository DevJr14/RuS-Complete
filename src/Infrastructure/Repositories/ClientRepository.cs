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
        private readonly IRepositoryAsync<Project, int> _projectRepo;

        public ClientRepository(IRepositoryAsync<Client, int> repository, IRepositoryAsync<Project, int> projectRepo)
        {
            _repository = repository;
            _projectRepo = projectRepo;
        }

        public async Task<bool> IsInUse(int clientId)
        {
            if (await _projectRepo.Entities.AnyAsync(p => p.ClientId == clientId))
            {
                return true;
            }
            return false;
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
