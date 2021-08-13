using RuS.Application.Features.Clients.Commands.AddEdit;
using RuS.Application.Interfaces.Repositories;
using RuS.Domain.Entities.Projects;
using System;
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

        public Task<bool> IsUniqueEntry(string name, int id = 0)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsUniqueEntry(AddEditClientCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
