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

namespace RuS.Application.Features.Categories.Commands
{
    public class DeleteCategoryCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteCategoryCommandHandler> _localizer;

        public DeleteCategoryCommandHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.Repository<Category>().GetByIdAsync(command.Id);
            if (category != null)
            {
                await _unitOfWork.Repository<Category>().DeleteAsync(category);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(category.Id, _localizer["Category Deleted."]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Category Not Found!"]);
            }
        }
    }
}
