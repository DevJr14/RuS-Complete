using Microsoft.EntityFrameworkCore;
using RuS.Application.Features.Projects.Commands.AddEdit;
using RuS.Application.Features.Tasks.Commands.AddEdit;
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
    public class TaskRepository : ITaskRepository
    {
        private readonly IRepositoryAsync<Task, int> _repository;
        private string _code;

        public TaskRepository(IRepositoryAsync<Task, int> repository)
        {
            _repository = repository;
        }

        public string GenerateProjectCode()
        {
            int maxid = maxid = _repository.Entities.DefaultIfEmpty().Max(task => task == null ? 0 : task.Id) + 1;

            _code = "TASK" + maxid;
            return _code;
        }

        public async Task<bool> IsUniqueEntry(AddEditTaskCommand command)
        {
            List<Task> tasks = await _repository.Entities.ToListAsync();
            if (command.Id == 0)
            {
                return tasks.Any(t => string.Equals(t.Name, command.Name, StringComparison.OrdinalIgnoreCase)
                    && t.ProjectId == command.ProjectId);
            }
            else
            {
                return tasks.Any(t => string.Equals(t.Name, command.Name, StringComparison.OrdinalIgnoreCase)
                    && t.ProjectId == command.ProjectId && t.Id != command.Id);
            }
        }
    }
}
