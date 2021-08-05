using Microsoft.AspNetCore.Components;
using RuS.Application.Features.Employees.Commands.AddEdit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RuS.Client.Pages.Core.Employee
{
    public partial class AddEditEmployee
    {
        [Parameter] public AddEditEmployeeCommand EmployeeCommand { get; set; } = new();
    }
}
