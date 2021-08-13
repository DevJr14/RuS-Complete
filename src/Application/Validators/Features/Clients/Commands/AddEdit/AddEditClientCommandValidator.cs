using FluentValidation;
using Microsoft.Extensions.Localization;
using RuS.Application.Features.Clients.Commands.AddEdit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Application.Validators.Features.Clients.Commands.AddEdit
{
    public class AddEditClientCommandValidator : AbstractValidator<AddEditClientCommand>
    {
        public AddEditClientCommandValidator(IStringLocalizer<AddEditClientCommandValidator> localizer)
        {
            RuleFor(c => c.Name)
                .Must(c => !string.IsNullOrWhiteSpace(c)).WithMessage(c => localizer["Name is required"])
                .MaximumLength(30).WithMessage(c => localizer["Name must not exceed 30 characters."])
                .Matches(@"^[a-zA-Z]+$").WithMessage(localizer["Name must contain alphabets only."]);
            RuleFor(c => c.ContactPerson)
                .MaximumLength(30).WithMessage(c => localizer["Contact person name must not exceed 30 characters."])
                .Matches(@"^[a-zA-Z]+$").WithMessage(c => localizer["Contact person name must contain alphabets only."]);
            RuleFor(c => c.TelephoneNo)
                .Length(10).WithMessage(c => localizer["Telephone no. must be 10 numbers long."])
                .Matches(@"^[0-9]+$").WithMessage(c => localizer["Telephone no. must contain numerics only."]);
            RuleFor(c => c.CellphoneNo)
                .Length(10).WithMessage(c => localizer["Cellphone no. must be 10 numbers long."])
                .Matches(@"^[0-9]+$").WithMessage(c => localizer["Cellphone no. must contain numerics only."]);
            RuleFor(c => c.Email)
                .EmailAddress().WithMessage(c => localizer["Invalid email format."])
                .MaximumLength(30).WithMessage(c => localizer["Email must not exceed 30 characters."]);
        }
    }
}
