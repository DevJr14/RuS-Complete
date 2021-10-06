using MediatR;
using Microsoft.Extensions.Localization;
using RuS.Application.Interfaces.Repositories;
using RuS.Domain.Entities.Projects;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RuS.Application.Features.Priorities.Commands
{
    public class DeletePriorityCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeletePriorityCommandHandler : IRequestHandler<DeletePriorityCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeletePriorityCommandHandler> _localizer;

        public DeletePriorityCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeletePriorityCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeletePriorityCommand command, CancellationToken cancellationToken)
        {
            var priority = await _unitOfWork.Repository<Priority>().GetByIdAsync(command.Id);
            if (priority != null)
            {
                await _unitOfWork.Repository<Priority>().DeleteAsync(priority);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(priority.Id, _localizer["Priority Deleted."]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Priority Not Found!"]);
            }
        }
    }
}
