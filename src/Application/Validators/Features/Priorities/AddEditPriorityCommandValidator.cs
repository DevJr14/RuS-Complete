using FluentValidation;
using Microsoft.Extensions.Localization;
using RuS.Application.Features.Priorities.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Application.Validators.Features.Priorities
{
    public class AddEditPriorityCommandValidator : AbstractValidator<AddEditPriorityCommand>
    {
        public AddEditPriorityCommandValidator(IStringLocalizer<AddEditPriorityCommandValidator> localizer)
        {
            RuleFor(p => p.Name)
                .Must(c => !string.IsNullOrEmpty(c)).WithMessage(c => localizer["Name is required"])
                .MaximumLength(30).WithMessage(c => localizer["Name must not exceed 30 characters."]);
            RuleFor(p => p.Description)
                .MaximumLength(60).WithMessage(p => localizer["Priority name must not exceed 60 characters."]);
        }
    }
}
