using FluentValidation;
using MediatR;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RuS.Application.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, Result<int>>
        where TRequest : IRequest<Result<int>>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validator)
        {
            _validators = validator;
        }
        public async Task<Result<int>> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<Result<int>> next)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                List<string> errors = new();
                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

                if (failures.Count != 0)
                {
                    foreach (var item in failures)
                    {
                        errors.Add(item.ErrorMessage);
                    }
                    return Result<int>.Fail(errors);
                }
            }
            return await next();
        }
    }
}
