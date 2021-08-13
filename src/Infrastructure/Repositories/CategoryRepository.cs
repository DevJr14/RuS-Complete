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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IRepositoryAsync<Category, int> _repository;

        public CategoryRepository(IRepositoryAsync<Category, int> repository)
        {
            _repository = repository;
        }

        public async Task<bool> IsUniqueEntry(string name, int id = 0)
        {
            List<Category> categories = await _repository.Entities.ToListAsync();
            if (id == 0)
            {
                return categories.Any(c => string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                return categories.Any(c => string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase) && c.Id != id);
            }
        }
    }
}
