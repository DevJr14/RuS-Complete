using MediatR;
using Microsoft.Extensions.Localization;
using RuS.Application.Interfaces.Repositories;
using RuS.Domain.Entities.Projects;
using RuS.Shared.Wrapper;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RuS.Application.Features.Discussions.Commands.Delete
{
    public class DeleteDiscussionCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteDiscussionCommandHandler : IRequestHandler<DeleteDiscussionCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteDiscussionCommandHandler> _localizer;

        public DeleteDiscussionCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteDiscussionCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteDiscussionCommand command, CancellationToken cancellationToken)
        {
            var discussion = await _unitOfWork.Repository<Discussion>().GetByIdAsync(command.Id);
            if(discussion != null)
            {
                await _unitOfWork.Repository<Discussion>().DeleteAsync(discussion);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(discussion.Id, _localizer["Comment Deleted."]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Comment Not Found."]);
            }
        }
    }
}
