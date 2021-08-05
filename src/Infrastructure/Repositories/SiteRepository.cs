using Microsoft.EntityFrameworkCore;
using RuS.Application.Interfaces.Repositories;
using RuS.Domain.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Infrastructure.Repositories
{
    public class SiteRepository : ISiteRepository
    {
        private readonly IRepositoryAsync<Site, int> _repository;

        public SiteRepository(IRepositoryAsync<Site, int> repository)
        {
            _repository = repository;
        }

        public async Task<bool> IsUniqueEntry(string name, int companyId, int id = 0)
        {
            List<Site> sites = await _repository.Entities.ToListAsync();
            if (id == 0)
            {
                return !sites.Any(s => string.Equals(s.Name, name, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                return !sites.Any(s => string.Equals(s.Name, name, StringComparison.OrdinalIgnoreCase)
                    && s.Id != id);
            }
        }
    }
}
