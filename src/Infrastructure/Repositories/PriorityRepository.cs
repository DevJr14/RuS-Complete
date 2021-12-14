using Microsoft.EntityFrameworkCore;
using RuS.Application.Interfaces.Repositories;
using RuS.Domain.Entities.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = RuS.Domain.Entities.Projects.Task;

namespace RuS.Infrastructure.Repositories
{
    public class PriorityRepository : IPriorityRepository
    {
        private readonly IRepositoryAsync<Priority, int> _repository;
        private readonly IRepositoryAsync<Project, int> _projectRepo;
        private readonly IRepositoryAsync<Task, int> _taskRepo;
        public PriorityRepository(IRepositoryAsync<Priority, int> repository, IRepositoryAsync<Project, int> projectRepo, IRepositoryAsync<Task, int> taskRepo)
        {
            _repository = repository;
            _projectRepo = projectRepo;
            _taskRepo = taskRepo;
        }

        public async Task<bool> IsInUse(int id)
        {
            if (await _projectRepo.Entities.AnyAsync(p => p.PriorityId == id)
                || await _taskRepo.Entities.AnyAsync(t => t.PriorityId == id))
            {
                return true;
            }
            return false;
        }

        public async Task<bool> IsUnique(string name, int id = 0)
        {
            List<Priority> priorities = await _repository.Entities.ToListAsync();
            if (id == 0)
            {
                return priorities.Any(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                return priorities.Any(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase) && p.Id != id);
            }
        }
    }
}
