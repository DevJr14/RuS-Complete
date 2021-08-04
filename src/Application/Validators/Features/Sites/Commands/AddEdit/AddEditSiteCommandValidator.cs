using FluentValidation;
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

        public AddEditSiteCommandValidator()
        {
            RuleFor(s => s.CompanyId)
                .NotEqual(0).WithMessage("Site must be linked to a Company");
            RuleFor(s => s.Name)
                .NotEmpty().WithMessage("Site name cannot be empty.")
                .MaximumLength(30).WithMessage("Site name must not exceed 30 characters.");
            RuleFor(s => s.Description)
                .MaximumLength(100).WithMessage("Site description must not exceed 100 characters.");
        }

    }
}
