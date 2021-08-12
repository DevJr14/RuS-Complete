using Microsoft.EntityFrameworkCore;
using RuS.Application.Interfaces.Repositories;
using RuS.Domain.Entities.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Infrastructure.Repositories
{
    public class PriorityRepository : IPriorityRepository
    {
        private readonly IRepositoryAsync<Priority, int> _repository;

        public PriorityRepository(IRepositoryAsync<Priority, int> repository)
        {
            _repository = repository;
        }

        public async Task<bool> IsUnique(string name, int id = 0)
        {
            List<Priority> priorities = await _repository.Entities.ToListAsync();
            if (id == 0)
            {
                return !priorities.Any(p => p.Name == name);
            }
            else
            {
                return !priorities.Any(p => p.Name == name && p.Id != id);
            }
        }
    }
}
