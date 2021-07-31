using MediatR;
using RuS.Application.Interfaces.Repositories;
using RuS.Domain.Entities.Core;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RuS.Application.Features.Companies.Commands.Delete
{
    public class DeleteCompanyCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        //private readonly IStringLocalizer<DeleteCompanyCommandHandler> _localizer;

        public DeleteCompanyCommandHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(DeleteCompanyCommand command, CancellationToken cancellationToken)
        {
            var company = await _unitOfWork.Repository<Company>().GetByIdAsync(command.Id);
            if (company != null)
            {
                await _unitOfWork.Repository<Company>().DeleteAsync(company);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(company.Id, "Company Deleted");
            }
            else
            {
                return await Result<int>.FailAsync("Company Not Found!");
            }
        }
    }
}
