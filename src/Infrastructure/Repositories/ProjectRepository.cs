using Microsoft.EntityFrameworkCore;
using RuS.Application.Features.Projects.Commands.AddEdit;
using RuS.Application.Interfaces.Repositories;
using RuS.Domain.Entities.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RuS.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly IRepositoryAsync<Project, int> _repository;
        private string _code;

        public ProjectRepository(IRepositoryAsync<Project, int> repository)
        {
            _repository = repository;
        }

        public string GenerateProjectCode()
        {
            int maxid = maxid = _repository.Entities.DefaultIfEmpty().Max(project => project == null ? 0 : project.Id) + 1;

            _code = "PROJ" + maxid;
            return _code;
        }

        public async Task<bool> IsUniqueEntry(AddEditProjectCommand command)
        {
            List<Project> projects = await _repository.Entities.ToListAsync();
            if (command.Id == 0)
{
                return projects.Any(p => string.Equals(p.Name, command.Name, StringComparison.OrdinalIgnoreCase)
                    && p.ClientId == command.ClientId);
            }
            else
            {
                return projects.Any(p => string.Equals(p.Name, command.Name, StringComparison.OrdinalIgnoreCase)
                    && p.ClientId == command.ClientId && p.Id != command.Id);
            }
        }
    }
}
