using MediatR;
using Microsoft.Extensions.Localization;
using RuS.Application.Interfaces.Repositories;
using RuS.Domain.Entities.Projects;
using RuS.Shared.Wrapper;
using System.Threading;
using System.Threading.Tasks;

namespace RuS.Application.Features.Teams.Commands.Delete
{
    public class DeleteTeamCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteTeamCommandHandler : IRequestHandler<DeleteTeamCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteTeamCommandHandler> _localizer;

        public DeleteTeamCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteTeamCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteTeamCommand command, CancellationToken cancellationToken)
{
            var team = await _unitOfWork.Repository<Team>().GetByIdAsync(command.Id);
            if (team != null)
            {
                await _unitOfWork.Repository<Team>().DeleteAsync(team);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(team.Id, _localizer["Team Deleted"]);
}
else
{
                return await Result<int>.FailAsync(_localizer[$"Team with Id: {command.Id} Not Found"]);
            }
        }
    }
}
