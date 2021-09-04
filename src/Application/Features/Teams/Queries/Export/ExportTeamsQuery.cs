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
using System.Threading;
using System.Threading.Tasks;

namespace RuS.Application.Features.Teams.Queries.Export
{
    public class ExportTeamsQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportTeamsQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportTeamsQueryHandler : IRequestHandler<ExportTeamsQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportTeamsQueryHandler> _localizer;

        public ExportTeamsQueryHandler(IExcelService excelService, IUnitOfWork<int> unitOfWork, IStringLocalizer<ExportTeamsQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportTeamsQuery request, CancellationToken cancellationToken)
        {
            var teamFilterSpec = new TeamFilterSpecification(request.SearchString);
            var teams = await _unitOfWork.Repository<Team>().Entities
                .Include(t => t.Project)
                .Specify(teamFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(teams, mappers: new Dictionary<string, Func<Team, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Name"], item => item.Name },
                { _localizer["Project"], item => item.Project.Name },
                { _localizer["Team Leader"], item => item.TeamLeaderId },
                { _localizer["Description"], item => item.Description }
            }, sheetName: _localizer["Teams"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
