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
    public class StatusRepository : IStatusRepository
    {
        private readonly IRepositoryAsync<Status, int> _repository;

        public StatusRepository(IRepositoryAsync<Status, int> repository)
        {
            _repository = repository;
        }

        public async Task<bool> IsUniqueEntry(string name, int id = 0)
        {
            List<Status> statuses = await _repository.Entities.ToListAsync();
            if (id == 0)
            {
                return !statuses.Any(s => s.Name == name);
            }
            else
            {
                return !statuses.Any(s => s.Name == name && s.Id != id);
            }
        }
    }
}
