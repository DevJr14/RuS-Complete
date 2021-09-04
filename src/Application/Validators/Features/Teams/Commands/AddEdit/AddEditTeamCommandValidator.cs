using FluentValidation;
using Microsoft.Extensions.Localization;
using RuS.Application.Features.Teams.Commands.AddEdit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Application.Validators.Features.Teams.Commands.AddEdit
{
    public class AddEditTeamCommandValidator : AbstractValidator<AddEditTeamCommand>
    {
        public AddEditTeamCommandValidator(IStringLocalizer<AddEditTeamCommandValidator> localizer)
        {
            RuleFor(t => t.Name)
                .Must(n => !string.IsNullOrEmpty(n)).WithMessage(p => localizer["Name is required"])
                .MaximumLength(30).WithMessage(c => localizer["Name must not exceed 30 characters."])
                .Matches(@"^[a-zA-Z0-9 ]+$").WithMessage(localizer["Special characters are not allowed."]);
            RuleFor(p => p.TeamLeaderId)
                .GreaterThan(0).WithMessage(c => localizer["Team must have a leader."]);
        }
    }
}
