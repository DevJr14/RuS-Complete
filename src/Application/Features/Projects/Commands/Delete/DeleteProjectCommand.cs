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

namespace RuS.Application.Features.Projects.Commands.Delete
{
    public class DeleteProjectCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteProjectCommandHandler> _localizer;

        public DeleteProjectCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteProjectCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteProjectCommand command, CancellationToken cancellationToken)
        {
            var project = await _unitOfWork.Repository<Project>().GetByIdAsync(command.Id);
            if (project != null)
            {
                await _unitOfWork.Repository<Project>().DeleteAsync(project);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(project.Id, _localizer["Project Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer[$"Project with Id: {command.Id} Not Found"]);
            }
        }
    }
}
