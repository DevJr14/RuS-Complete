using FluentValidation;
using Microsoft.Extensions.Localization;
using RuS.Application.Features.Categories.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Application.Validators.Features.Categories
{
    public class AddEditCategoryCommandValidator : AbstractValidator<AddEditCategoryCommand>
    {
        public AddEditCategoryCommandValidator(IStringLocalizer<AddEditCategoryCommandValidator> localizer)
        {
            RuleFor(c => c.Name)
                .Must(c => !string.IsNullOrWhiteSpace(c)).WithMessage(c => localizer["Name is required"])
                .MaximumLength(30).WithMessage(c => localizer["Name must not exceed 30 characters."]);
            RuleFor(c => c.Description)
                .MaximumLength(60).WithMessage(c => localizer["Name must not exceed 60 characters."]);
        }
    }
}
