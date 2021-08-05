﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Application.Interfaces.Repositories
{
    public interface ISiteRepository
    {
        Task<bool> IsUniqueEntry(string name, int companyId, int id = 0);
    }
}
