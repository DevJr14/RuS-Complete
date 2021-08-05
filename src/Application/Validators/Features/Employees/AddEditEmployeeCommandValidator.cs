using FluentValidation;
using RuS.Application.Features.Employees.Commands.AddEdit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Application.Validators.Features.Employees
{
    public class AddEditEmployeeCommandValidator : AbstractValidator<AddEditEmployeeCommand>
    {
        public AddEditEmployeeCommandValidator()
        {
            RuleFor(e => e.FirstName)
                .NotEmpty().WithMessage("Firstname is required.")
                .MaximumLength(30).WithMessage("Firstname must not exceed 30 charactors.");

            RuleFor(e => e.LastName)
                .NotEmpty().WithMessage("Lastname is required.")
                .MaximumLength(30).WithMessage("Lastname must not exceed 30 charactors.");
        }
    }
}
