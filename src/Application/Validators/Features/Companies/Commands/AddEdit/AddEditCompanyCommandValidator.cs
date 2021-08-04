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
        public AddEditCompanyCommandValidator()
        {

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Company name cannot be empty.")
                .MaximumLength(30).WithMessage("Company name must not exceed 30 characters.");
        }
    }
}
