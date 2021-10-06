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

namespace RuS.Application.Features.Statuses.Queries
{
    public class ExportStatusesQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportStatusesQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportStatusQueryHandler : IRequestHandler<ExportStatusesQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportStatusQueryHandler> _localizer;

        public ExportStatusQueryHandler(IUnitOfWork<int> unitOfWork, IExcelService excelService, IStringLocalizer<ExportStatusQueryHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _excelService = excelService;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportStatusesQuery request, CancellationToken cancellationToken)
        {
            var statusFilterSpec = new StatusFilterSpecification(request.SearchString);
            var statuses = await _unitOfWork.Repository<Status>().Entities
                .Specify(statusFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(statuses, mappers: new Dictionary<string, Func<Status, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Name"], item => item.Name },
                { _localizer["Description"], item => item.Description }
            }, sheetName: _localizer["Statuses"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
