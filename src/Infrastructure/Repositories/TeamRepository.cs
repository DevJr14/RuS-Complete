using Microsoft.EntityFrameworkCore;
using RuS.Application.Features.Teams.Commands.AddEdit;
using RuS.Application.Interfaces.Repositories;
using RuS.Domain.Entities.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Infrastructure.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly IRepositoryAsync<Team, int> _repository;

        public TeamRepository(IRepositoryAsync<Team, int> repository)
        {
            _repository = repository;
        }

        public async Task<bool> IsUniqueEntry(AddEditTeamCommand command)
        {
            List<Team> teams = await _repository.Entities.ToListAsync();
            if (command.Id == 0)
            {
                return teams.Any(t => string.Equals(t.Name, command.Name, StringComparison.OrdinalIgnoreCase)
                    && t.ProjectId == command.ProjectId);
            }
            else
            {
                return teams.Any(t => string.Equals(t.Name, command.Name, StringComparison.OrdinalIgnoreCase)
                    && t.ProjectId == command.ProjectId && t.Id != command.Id);
            }
        }
    }
}
