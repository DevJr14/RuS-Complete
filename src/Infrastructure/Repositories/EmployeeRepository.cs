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
    }
}
