using FluentValidation;
using Microsoft.Extensions.Localization;
using RuS.Application.Features.Sites.Commands.AddEdit;
using RuS.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RuS.Application.Validators.Features.Sites.Commands.AddEdit
{
    public class AddEditSiteCommandValidator : AbstractValidator<AddEditSiteCommand>
    {

        public AddEditSiteCommandValidator(IStringLocalizer<AddEditSiteCommandValidator> localizer)
        {
            RuleFor(s => s.CompanyId)
                .NotEqual(0).WithMessage(s => localizer["Site must be linked to a Company"]);
            RuleFor(s => s.Name)
                .Must(s => !string.IsNullOrWhiteSpace(s)).WithMessage(s => localizer["Name is required"])
                .MaximumLength(30).WithMessage(c => localizer["Name must not exceed 30 characters."]);
            RuleFor(s => s.Description)
                .MaximumLength(100).WithMessage(s => localizer["Site description must not exceed 100 characters."]);
        }

    }
}
