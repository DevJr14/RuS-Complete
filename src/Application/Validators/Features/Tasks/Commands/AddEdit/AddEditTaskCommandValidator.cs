﻿using FluentValidation;
using Microsoft.Extensions.Localization;
using RuS.Application.Features.Tasks.Commands.AddEdit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Application.Validators.Features.Tasks.Commands.AddEdit
{
    public class AddEditTaskCommandValidator : AbstractValidator<AddEditTaskCommand>
    {
        public AddEditTaskCommandValidator(IStringLocalizer<AddEditTaskCommandValidator> localizer)
        {
            RuleFor(t => t.Name)
                .Must(n => !string.IsNullOrEmpty(n)).WithMessage(p => localizer["Name is required"])
                .MaximumLength(30).WithMessage(c => localizer["Name must not exceed 30 characters."])
                .Matches(@"^[a-zA-Z0-9 ]+$").WithMessage(localizer["Special characters are not allowed."]);
            RuleFor(p => p.PriorityId)
                .GreaterThan(0).WithMessage(c => localizer["Task must have Priority."]);
            RuleFor(p => p.StatusId)
                .GreaterThan(0).WithMessage(c => localizer["Task must have Status."]);
            RuleFor(p => p.CategoryId)
                .GreaterThan(0).WithMessage(c => localizer["Task must have Category."]);
            RuleFor(p => p.End)
                .GreaterThanOrEqualTo(p => p.Start).WithMessage(c => localizer["End should be greater than or equal Start."]);
        }
    }
}
