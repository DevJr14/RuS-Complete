using MediatR;
using Microsoft.Extensions.Localization;
using RuS.Application.Interfaces.Repositories;
using RuS.Domain.Entities.Core;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace RuS.Application.Features.Employees.Commands.Delete
{
    public class DeleteEmployeeCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteEmployeeCommandHandler> _localizer;

        public DeleteEmployeeCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteEmployeeCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteEmployeeCommand command, CancellationToken cancellationToken)
        {
            var employee = await _unitOfWork.Repository<Employee>().GetByIdAsync(command.Id);
            if (employee != null)
            {
                await _unitOfWork.Repository<Employee>().DeleteAsync(employee);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(employee.Id, _localizer["Employee Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Employee Not Found!"]);
            }
        }
    }
}
