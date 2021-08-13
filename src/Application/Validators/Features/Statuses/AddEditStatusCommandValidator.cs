using FluentValidation;
using Microsoft.Extensions.Localization;
using RuS.Application.Features.Statuses.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Application.Validators.Features.Statuses
{
    public class AddEditStatusCommandValidator : AbstractValidator<AddEditStatusCommand>
    {
        public AddEditStatusCommandValidator(IStringLocalizer<AddEditStatusCommandValidator> localizer)
        {
            RuleFor(s => s.Name)
                .Must(s => !string.IsNullOrWhiteSpace(s)).WithMessage(s => localizer["Name is required!"])
                .MaximumLength(30).WithMessage(s => localizer["Status name must not exceed 30 characters."]);
            RuleFor(s => s.Description)
                .MaximumLength(60).WithMessage(s => localizer["Status name must not exceed 60 characters."]);
        }
    }
}
