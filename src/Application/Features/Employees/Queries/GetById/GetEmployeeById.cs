using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RuS.Application.Interfaces.Repositories;
using RuS.Domain.Entities.Core;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RuS.Application.Features.Employees.Queries.GetById
{
    public class GetEmployeeById : IRequest<Result<GetEmployeeResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetEmployeeByIdHandler : IRequestHandler<GetEmployeeById, Result<GetEmployeeResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetEmployeeByIdHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetEmployeeResponse>> Handle(GetEmployeeById request, CancellationToken cancellationToken)
        {
            var employee = await _unitOfWork.Repository<Employee>().Entities
                .Where(c => c.Id == request.Id)
                .Include(c => c.Company)
                .FirstOrDefaultAsync();
            var mappedEmployee = _mapper.Map<GetEmployeeResponse>(employee);
            return await Result<GetEmployeeResponse>.SuccessAsync(mappedEmployee);
        }
    }
}
