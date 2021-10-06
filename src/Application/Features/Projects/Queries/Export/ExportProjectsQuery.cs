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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RuS.Application.Features.Projects.Queries.Export
{
    public class ExportProjectsQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportProjectsQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportProjectsQueryHandler : IRequestHandler<ExportProjectsQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportProjectsQueryHandler> _localizer;

        public ExportProjectsQueryHandler(IUnitOfWork<int> unitOfWork, IExcelService excelService, IStringLocalizer<ExportProjectsQueryHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _excelService = excelService;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportProjectsQuery request, CancellationToken cancellationToken)
        {
            var projectFilterSpec = new ProjectFilterSpecification(request.SearchString);
            var projects = await _unitOfWork.Repository<Project>().Entities
                .Include(p => p.Site)
                .Include(p => p.Status)
                .Include(p => p.Category)
                .Include(p => p.Priority)
                .Include(p => p.Client)
                .Specify(projectFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(projects, mappers: new Dictionary<string, Func<Project, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Name"], item => item.Name },
                { _localizer["Site"], item => item.Site.Name },
                { _localizer["Status"], item => item.Status.Name },
                { _localizer["Category"], item => item.Category.Name },
                { _localizer["Priority"], item => item.Priority.Name },
                { _localizer["Client"], item => item.Client.Name },
                { _localizer["Description"], item => item.Description },
                { _localizer["Scope Of Work"], item => item.ScopeOfWork },
                { _localizer["Start Date"], item => DateTime.SpecifyKind((DateTime)item.Start, DateTimeKind.Utc).ToLocalTime().ToString("G", CultureInfo.CurrentCulture) },
                { _localizer["End Date"], item => DateTime.SpecifyKind((DateTime)item.End, DateTimeKind.Utc).ToLocalTime().ToString("G", CultureInfo.CurrentCulture) },
                { _localizer["Actual Start"], item => DateTime.SpecifyKind(item.ActualStart.GetValueOrDefault(), DateTimeKind.Utc).ToLocalTime().ToString("G", CultureInfo.CurrentCulture) },
                { _localizer["Actual End"], item => DateTime.SpecifyKind(item.ActualEnd.GetValueOrDefault(), DateTimeKind.Utc).ToLocalTime().ToString("G", CultureInfo.CurrentCulture) },
            }, sheetName: _localizer["Projects"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
