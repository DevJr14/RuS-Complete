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
    public class CompanyRepository : ICompanyRepository
    {
        private readonly IRepositoryAsync<Company, int> _repository;

        public CompanyRepository(IRepositoryAsync<Company, int> repository)
        {
            _repository = repository;
        }

        public async Task<bool> IsUniqueEntry(string name, string regNo, int id = 0)
        {
            List<Company> companies = await _repository.Entities.ToListAsync();
            if(id == 0)
            {
                return !companies.Any(c => string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase)
                    && string.Equals(c.RegistrationNo, regNo, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                return !companies.Any(c => string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase)
                    && string.Equals(c.RegistrationNo, regNo, StringComparison.OrdinalIgnoreCase)
                    && c.Id != id);
            }
        }
    }
}
