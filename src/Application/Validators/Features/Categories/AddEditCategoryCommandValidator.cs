using FluentValidation;
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
        public AddEditCategoryCommandValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Category name cannot be empty.")
                .MaximumLength(30).WithMessage("Category name must not exceed 30 characters.");
            RuleFor(c => c.Description)
                .MaximumLength(60).WithMessage("Category name must not exceed 60 characters.");
        }
    }
}
