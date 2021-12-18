using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RuS.Application.Extensions;
using RuS.Application.Interfaces.Repositories;
using RuS.Application.Interfaces.Services;
using RuS.Application.Specifications.Core;
using RuS.Domain.Entities.Core;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RuS.Application.Features.Sites.Queries.Export
{
    public class ExportSitesQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }
        public int CompanyId { get; set; }

        public ExportSitesQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportSitesQueryHandler : IRequestHandler<ExportSitesQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportSitesQueryHandler> _localizer;

        public ExportSitesQueryHandler(IUnitOfWork<int> unitOfWork, IExcelService excelService, IStringLocalizer<ExportSitesQueryHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _excelService = excelService;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportSitesQuery request, CancellationToken cancellationToken)
        {
            var siteFilterSpec = new SiteFilterSpecification(request.SearchString, request.CompanyId);
            var sites = await _unitOfWork.Repository<Site>().Entities
                .Include(s => s.Company)
                .Specify(siteFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(sites, mappers: new Dictionary<string, Func<Site, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Name"], item => item.Name },
                { _localizer["Description"], item => item.Description },
                { _localizer["Company Id"], item => item.CompanyId },
                { _localizer["Company Name"], item => item.Company.Name }
            }, sheetName: _localizer["Brands"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
