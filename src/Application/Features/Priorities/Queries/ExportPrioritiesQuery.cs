using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RuS.Application.Extensions;
using RuS.Application.Interfaces.Repositories;
using RuS.Application.Interfaces.Services;
using RuS.Application.Specifications.Project;
using RuS.Domain.Entities.Projects;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RuS.Application.Features.Priorities.Queries
{
    public class ExportPrioritiesQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }
        public ExportPrioritiesQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportPrioritiesQueryHandler : IRequestHandler<ExportPrioritiesQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportPrioritiesQueryHandler> _localizer;

        public ExportPrioritiesQueryHandler(IUnitOfWork<int> unitOfWork, IExcelService excelService, IStringLocalizer<ExportPrioritiesQueryHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _excelService = excelService;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportPrioritiesQuery request, CancellationToken cancellationToken)
        {
            var priorityFilterSpec = new PriorityFilterSpecification(request.SearchString);
            var priorities = await _unitOfWork.Repository<Priority>().Entities
                .Specify(priorityFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(priorities, mappers: new Dictionary<string, Func<Priority, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Name"], item => item.Name },
                { _localizer["Description"], item => item.Description }
            }, sheetName: _localizer["Priorities"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
