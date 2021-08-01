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

namespace RuS.Application.Features.Sites.Commands.Delete
{
    public class DeleteSiteCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteSiteCommandHandler : IRequestHandler<DeleteSiteCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteSiteCommandHandler> _localizer;

        public DeleteSiteCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteSiteCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteSiteCommand command, CancellationToken cancellationToken)
        {
            var site = await _unitOfWork.Repository<Site>().GetByIdAsync(command.Id);
            if (site != null)
            {
                await _unitOfWork.Repository<Site>().DeleteAsync(site);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(site.Id, _localizer["Site Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Site Not Found!"]);
            }
        }
    }
}
