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

namespace RuS.Application.Features.Statuses.Commands
{
    public class DeleteStatusCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteStatusCommandHandler : IRequestHandler<DeleteStatusCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteStatusCommandHandler> _localizer;

        public DeleteStatusCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteStatusCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteStatusCommand command, CancellationToken cancellationToken)
        {
            var status = await _unitOfWork.Repository<Status>().GetByIdAsync(command.Id);
            if (status != null)
            {
                await _unitOfWork.Repository<Status>().DeleteAsync(status);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(status.Id, _localizer["Status Deleted."]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Status Not Found!"]);
            }
        }
    }
}
