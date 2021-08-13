using MediatR;
using Microsoft.Extensions.Localization;
using RuS.Application.Interfaces.Repositories;
using RuS.Domain.Entities.Projects;
using RuS.Shared.Wrapper;
using System.Threading;
using System.Threading.Tasks;

namespace RuS.Application.Features.Clients.Commands.Delete
{
    public class DeleteClientCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteClientCommandHandler : IRequestHandler<DeleteClientCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteClientCommandHandler> _localizer;

        public DeleteClientCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteClientCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteClientCommand command, CancellationToken cancellationToken)
        {
            var client = await _unitOfWork.Repository<Client>().GetByIdAsync(command.Id);
            if (client != null)
            {
                await _unitOfWork.Repository<Client>().DeleteAsync(client);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(client.Id, _localizer["Client Deleted."]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Client Not Found!"]);
            }
        }
    }
}
