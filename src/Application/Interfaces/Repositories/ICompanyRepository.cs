using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Application.Interfaces.Repositories
{
    public interface ICompanyRepository
    {
        Task<bool> IsUniqueEntry(string name, string regNo, int id = 0);
    }
}
