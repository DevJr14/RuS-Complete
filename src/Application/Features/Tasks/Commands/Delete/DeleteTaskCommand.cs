using MediatR;
using Microsoft.Extensions.Localization;
using RuS.Application.Interfaces.Repositories;
using RuS.Domain.Entities.Projects;
using RuS.Shared.Wrapper;
using System;
using System.Threading;
using System.Threading.Tasks;
using Task = RuS.Domain.Entities.Projects.Task;

namespace RuS.Application.Features.Tasks.Commands.Delete
{
    public class DeleteTaskCommand : IRequest<Result<int>>
    { 
        public int Id { get; set; }
    }

    internal class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteTaskCommandHandler> _localizer;

        public DeleteTaskCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteTaskCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteTaskCommand command, CancellationToken cancellationToken)
        {
            var task = await _unitOfWork.Repository<Task>().GetByIdAsync(command.Id);
            if (task != null)
            {
                await _unitOfWork.Repository<Task>().DeleteAsync(task);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(task.Id, _localizer["Task Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer[$"Task with Id: {command.Id} Not Found"]);
            }
        }
    }
}
