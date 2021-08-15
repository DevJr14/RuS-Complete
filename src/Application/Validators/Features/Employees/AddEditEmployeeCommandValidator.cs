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
                .Must(f => !string.IsNullOrWhiteSpace(f)).WithMessage(e => localizer["Firstname is required"])
                .MaximumLength(30).WithMessage(e => localizer["Firstname must not exceed 30 characters."])
                .Matches(@"^[a-zA-Z]+$").WithMessage(e => localizer["Firstname must contain alphabets only."]);

            RuleFor(e => e.LastName)
                .Must(l => !string.IsNullOrWhiteSpace(l)).WithMessage(e => localizer["Lastname is required"])
                .MaximumLength(30).WithMessage(e => localizer["Lastname must not exceed 30 characters."])
                .Matches(@"^[a-zA-Z]+$").WithMessage(e => localizer["Lastname must contain alphabets only."]);

            RuleFor(e => e.CompanyId)
                .NotEqual(0).WithMessage(e => localizer["Employee must be linked to a Company"]);

            RuleFor(e => e.CellphoneNo)
                .Length(10).WithMessage(e => localizer["Cellphone no. must be 10 numbers long."])
                .Matches(@"^[0-9]+$").WithMessage(e => localizer["Cellphone no. must contain numerics only."]);

            RuleFor(e => e.Email)
                .EmailAddress().WithMessage(e => localizer["Invalid email format."])
                .MaximumLength(30).WithMessage(e => localizer["Email must not exceed 30 characters."]);

            RuleFor(e => e.DateOfBirth)
                .Must(d => d != default(DateTime)).WithMessage(e => localizer["Date of Birth is required"]);

            RuleFor(e => e.Gender)
                .Must(g => !string.IsNullOrWhiteSpace(g)).WithMessage(e => localizer["Gender is required"]);
        }
    }
}
