using FluentValidation;
using Microsoft.Extensions.Localization;
using RuS.Application.Features.Employees.Commands.AddEdit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Application.Validators.Features.Employees
{
    public class AddEditEmployeeCommandValidator : AbstractValidator<AddEditEmployeeCommand>
    {
        public AddEditEmployeeCommandValidator(IStringLocalizer<AddEditEmployeeCommandValidator> localizer)
        {
            RuleFor(e => e.FirstName)
                .Must(c => !string.IsNullOrWhiteSpace(c)).WithMessage(c => localizer["Firstname is required"])
                .MaximumLength(30).WithMessage(c => localizer["Firstname must not exceed 30 characters."])
                .Matches(@"^[a-zA-Z]+$").WithMessage(localizer["Firstname must contain alphabets only."]);

            RuleFor(e => e.LastName)
                .Must(c => !string.IsNullOrWhiteSpace(c)).WithMessage(c => localizer["Lastname is required"])
                .MaximumLength(30).WithMessage(c => localizer["Lastname must not exceed 30 characters."])
                .Matches(@"^[a-zA-Z]+$").WithMessage(localizer["Lastname must contain alphabets only."]);
        }
    }
}
