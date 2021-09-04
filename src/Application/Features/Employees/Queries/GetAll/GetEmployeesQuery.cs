using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using RuS.Application.Features.Employees.Queries.GetById;
using RuS.Application.Interfaces.Repositories;
using RuS.Domain.Entities.Core;
using RuS.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RuS.Application.Features.Employees.Queries.GetAll
{
    public class GetEmployeesQuery : IRequest<Result<List<GetEmployeeResponse>>>
    {
    }

    internal class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, Result<List<GetEmployeeResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetEmployeesQueryHandler> _localizer;

        public GetEmployeesQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<GetEmployeesQueryHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<List<GetEmployeeResponse>>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
        {
            var employees = await _unitOfWork.Repository<Employee>().GetAllAsync();
            if(employees.Count > 0)
            {
                var mappedEmployees = _mapper.Map<List<GetEmployeeResponse>>(employees);
                return await Result<List<GetEmployeeResponse>>.SuccessAsync(mappedEmployees);
            }
            else
            {
                return await Result<List<GetEmployeeResponse>>.FailAsync(_localizer["No Employees."]);
            }
        }
    }
}
