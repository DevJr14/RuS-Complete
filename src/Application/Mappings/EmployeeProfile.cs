using AutoMapper;
using RuS.Application.Features.Employees.Commands.AddEdit;
using RuS.Application.Features.Employees.Queries.GetAllPaged;
using RuS.Application.Features.Employees.Queries.GetById;
using RuS.Domain.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Application.Mappings
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<AddEditEmployeeCommand, Employee>().ReverseMap();
            CreateMap<GetAllPagedEmployeesResponse, Employee>().ReverseMap();
            CreateMap<GetEmployeeResponse, Employee>().ReverseMap();
        }
    }
}
