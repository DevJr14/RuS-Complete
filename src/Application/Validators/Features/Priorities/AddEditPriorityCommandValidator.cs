using FluentValidation;
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
        public AddEditPriorityCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Priority name cannot be empty.")
                .MaximumLength(30).WithMessage("Priority name must not exceed 30 characters.");
            RuleFor(p => p.Description)
                .MaximumLength(60).WithMessage("Priority name must not exceed 60 characters.");
        }
    }
}
