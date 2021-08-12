using FluentValidation;
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
        public AddEditStatusCommandValidator()
        {
            RuleFor(s => s.Name)
                .NotEmpty().WithMessage("Status name cannot be empty.")
                .MaximumLength(30).WithMessage("Status name must not exceed 30 characters.");
            RuleFor(s => s.Description)
                .MaximumLength(60).WithMessage("Status name must not exceed 60 characters.");
        }
    }
}
