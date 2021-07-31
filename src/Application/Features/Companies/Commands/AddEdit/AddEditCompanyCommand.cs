using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using RuS.Application.Interfaces.Repositories;
using RuS.Domain.Entities.Core;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RuS.Application.Features.Companies.Commands.AddEdit
{
    public class AddEditCompanyCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Company name is required.")]
        public string Name { get; set; }
        public string ShortTitle { get; set; }
        public string RegistrationNo { get; set; }
        public DateTime? RegistrationDate { get; set; }
    }

    public class AddEditCompanyCommandHnadler : IRequestHandler<AddEditCompanyCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<AddEditCompanyCommandHnadler> _localizer;

        public AddEditCompanyCommandHnadler(IStringLocalizer<AddEditCompanyCommandHnadler> localizer, IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _localizer = localizer;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(AddEditCompanyCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var company = _mapper.Map<Company>(command);
                await _unitOfWork.Repository<Company>().AddAsync(company);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(company.Id, "Company Saved");
            }
            else
            {
                var company = await _unitOfWork.Repository<Company>().GetByIdAsync(command.Id);
                if (company != null)
                {
                    company.Name = command.Name ?? company.Name;
                    company.ShortTitle = command.ShortTitle ?? company.ShortTitle;
                    company.RegistrationNo = command.RegistrationNo ?? company.RegistrationNo;
                    company.RegistrationDate = command.RegistrationDate ?? company.RegistrationDate;
                    await _unitOfWork.Repository<Company>().UpdateAsync(company);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(company.Id, "Company Updated");
                }
                else
                {
                    return await Result<int>.FailAsync("Company Not Found!");
                }
            }
        }
    }
}
