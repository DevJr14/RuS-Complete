using FluentValidation;
using Microsoft.Extensions.Localization;
using RuS.Application.Features.Projects.Commands.AddEdit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Application.Validators.Features.Projects.Commans.AddEdit
{
    public class AddEditProjectCommandValidator: AbstractValidator<AddEditProjectCommand>
    {
        public AddEditProjectCommandValidator(IStringLocalizer<AddEditProjectCommandValidator> localizer)
        {
            RuleFor(p => p.Name)
                .Must(n => !string.IsNullOrEmpty(n)).WithMessage(p => localizer["Name is required"])
                .MaximumLength(30).WithMessage(c => localizer["Name must not exceed 30 characters."])
                .Matches(@"^[a-zA-Z0-9 ]+$").WithMessage(localizer["Special characters are not allowed."]);
            RuleFor(p => p.SiteId)
                .GreaterThan(0).WithMessage(c => localizer["Project must be linked to Site."]);
            RuleFor(p => p.PriorityId)
                .GreaterThan(0).WithMessage(c => localizer["Project must have Priority."]);
            RuleFor(p => p.StatusId)
                .GreaterThan(0).WithMessage(c => localizer["Project must have Status."]);
            RuleFor(p => p.CategoryId)
                .GreaterThan(0).WithMessage(c => localizer["Project must have Category."]);
            RuleFor(p => p.ClientId)
                .GreaterThan(0).WithMessage(c => localizer["Project must have Client."]);
            RuleFor(p => p.End)
                .GreaterThanOrEqualTo(p => p.Start).WithMessage(c => localizer["End should be greater than or equal Start."]);
            RuleFor(p => p.ActualEnd)
                .GreaterThanOrEqualTo(p => p.ActualStart).WithMessage(c => localizer["End should be greater than or equal Start."]);
        }
    }
}
