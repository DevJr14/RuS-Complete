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
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IRepositoryAsync<Employee, int> _repository;
        private string _employeeNo;

        public EmployeeRepository(IRepositoryAsync<Employee, int> repository)
        {
            _repository = repository;
        }

        public string GenerateEmployeeNo()
        {
            int maxid = maxid = _repository.Entities.DefaultIfEmpty().Max(emp => emp == null ? 0 : emp.Id) + 1;

            _employeeNo = "EMP" + maxid;
            return _employeeNo;
        }

        public async Task<bool> IsUniqueEntry(string fName, string lName, DateTime dBirth, int companyId, int id = 0)
        {
            List<Employee> employees = await _repository.Entities.ToListAsync();
            if (id == 0)
            {
                return !employees.Any(e => string.Equals(e.FirstName, fName, StringComparison.OrdinalIgnoreCase)
                    && string.Equals(e.LastName, lName, StringComparison.OrdinalIgnoreCase)
                    && e.DateOfBirth == dBirth
                    && e.CompanyId == companyId);
            }
            else
            {
                return !employees.Any(e => string.Equals(e.FirstName, fName, StringComparison.OrdinalIgnoreCase)
                    && string.Equals(e.LastName, lName, StringComparison.OrdinalIgnoreCase)
                    && e.DateOfBirth == dBirth
                    && e.CompanyId == companyId && e.Id != id);
            }
        }
    }
}
