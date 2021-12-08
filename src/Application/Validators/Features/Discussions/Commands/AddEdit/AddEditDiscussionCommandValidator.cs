using FluentValidation;
using Microsoft.Extensions.Localization;
using RuS.Application.Features.Discussions.Commands.AddEdit;

namespace RuS.Application.Validators.Features.Discussions.Commands.AddEdit
{
    public class AddEditDiscussionCommandValidator : AbstractValidator<AddEditDiscussionCommand>
    {
        public AddEditDiscussionCommandValidator(IStringLocalizer<AddEditDiscussionCommandValidator> localizer)
        {
            RuleFor(d => d.Comment)
                .Must(t => !string.IsNullOrEmpty(t)).WithMessage(d => localizer["Comment is required."])
                .MaximumLength(250).WithMessage(d => localizer["Comment must not exceed 250 characters."]);   
            RuleFor(d => d.ProjectId)
                .GreaterThan(0).WithMessage(x => localizer["Project is required."]);
        }
    }
}
