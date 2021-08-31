using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RuS.Application.Extensions;
using RuS.Application.Interfaces.Repositories;
using RuS.Application.Interfaces.Services;
using RuS.Application.Specifications.Project;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Task = RuS.Domain.Entities.Projects.Task;

namespace RuS.Application.Features.Tasks.Queries.Export
{
    public class ExportTasksQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportTasksQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportTasksQueryHandler : IRequestHandler<ExportTasksQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportTasksQueryHandler> _localizer;

        public ExportTasksQueryHandler(IExcelService excelService, IUnitOfWork<int> unitOfWork, IStringLocalizer<ExportTasksQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportTasksQuery request, CancellationToken cancellationToken)
        {
            var taskFilterSpec = new TaskFilterSpecification(request.SearchString);
            var tasts = await _unitOfWork.Repository<Task>().Entities
                .Include(t => t.Project)
                .Include(t => t.Status)
                .Include(t => t.Category)
                .Include(t => t.Priority)
                .Specify(taskFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(tasts, mappers: new Dictionary<string, Func<Task, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Name"], item => item.Name },
                { _localizer["Project"], item => item.Project.Name },
                { _localizer["Status"], item => item.Status.Name },
                { _localizer["Category"], item => item.Category.Name },
                { _localizer["Priority"], item => item.Priority.Name },
                { _localizer["Description"], item => item.Description },
                { _localizer["Start Date"], item => DateTime.SpecifyKind((DateTime)item.Start, DateTimeKind.Utc).ToLocalTime().ToString("G", CultureInfo.CurrentCulture) },
                { _localizer["End Date"], item => DateTime.SpecifyKind((DateTime)item.End, DateTimeKind.Utc).ToLocalTime().ToString("G", CultureInfo.CurrentCulture) }
            }, sheetName: _localizer["Tasks"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
