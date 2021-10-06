using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Application.Interfaces.Repositories
{
    public interface IEmployeeRepository
    {
        string GenerateEmployeeNo();

        Task<bool> IsUniqueEntry(string fName, string lName, DateTime dBirth, int companyId, int id = 0);
    }
}
