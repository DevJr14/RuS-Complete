﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Application.Interfaces.Repositories
{
    public interface IPriorityRepository
    {
        Task<bool> IsUnique(string name, int id = 0);
        Task<bool> IsInUse(int id);
    }
}
