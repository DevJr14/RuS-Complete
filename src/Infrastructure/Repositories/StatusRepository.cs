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
    public class StatusRepository : IStatusRepository
    {
        private readonly IRepositoryAsync<Status, int> _repository;
        private readonly IRepositoryAsync<Project, int> _projectRepo;
        private readonly IRepositoryAsync<Task, int> _taskRepo;
        public StatusRepository(IRepositoryAsync<Status, int> repository, IRepositoryAsync<Project, int> projectRepo, IRepositoryAsync<Task, int> taskRepo)
        {
            _repository = repository;
            _projectRepo = projectRepo;
            _taskRepo = taskRepo;
        }

        public async Task<bool> IsInUse(int id)
        {
            if (await _projectRepo.Entities.AnyAsync(p => p.StatusId == id)
                || await _taskRepo.Entities.AnyAsync(t => t.StatusId == id))
            {
                return true;
            }
            return false;
        }

        public async Task<bool> IsUniqueEntry(string name, int id = 0)
        {
            List<Status> statuses = await _repository.Entities.ToListAsync();
            if (id == 0)
            {
                return statuses.Any(s => string.Equals(s.Name, name, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                return statuses.Any(s => string.Equals(s.Name, name, StringComparison.OrdinalIgnoreCase) && s.Id != id);
            }
        }
    }
}
