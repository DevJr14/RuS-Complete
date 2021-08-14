using FluentValidation;
using Microsoft.Extensions.Localization;
using RuS.Application.Features.Companies.Commands.AddEdit;

namespace RuS.Application.Validators.Features.Companies.Commands.AddEdit
{
    public class AddEditCompanyCommandValidator : AbstractValidator<AddEditCompanyCommand>
    {
        public AddEditCompanyCommandValidator(IStringLocalizer<AddEditCompanyCommandValidator> localizer)
        {

            RuleFor(c => c.Name)
                .Must(c => !string.IsNullOrWhiteSpace(c)).WithMessage(c => localizer["Company Name is required"])
                .MaximumLength(30).WithMessage(c => localizer["Name must not exceed 30 characters."]);
        }
    }
}
