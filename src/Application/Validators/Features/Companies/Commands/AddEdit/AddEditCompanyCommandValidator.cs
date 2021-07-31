using FluentValidation;
using RuS.Application.Features.Companies.Commands.AddEdit;
using RuS.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RuS.Application.Validators.Features.Companies.Commands.AddEdit
{
    public class AddEditCompanyCommandValidator : AbstractValidator<AddEditCompanyCommand>
    {
        private readonly ICompanyRepository _companyRepository;
        public AddEditCompanyCommandValidator(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;

            RuleFor(c => c)
                .MustAsync(UniqueEntry).WithMessage("Company already exists.");
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Company name cannot be empty.")
                .MaximumLength(30).WithMessage("Company name must not exceed 30 characters.");
        }

        private async Task<bool> UniqueEntry(AddEditCompanyCommand command, CancellationToken cancelationToken)
        {
            return await _companyRepository.IsUniqueEntry(command.Name, command.RegistrationNo, command.Id);
        }
    }
}
