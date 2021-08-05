using MediatR;
using Microsoft.EntityFrameworkCore;
using RuS.Application.Extensions;
using RuS.Application.Interfaces.Repositories;
using RuS.Application.Interfaces.Services;
using RuS.Application.Specifications.Core;
using RuS.Domain.Entities.Core;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RuS.Application.Features.Companies.Queries.Export
{
    public class ExportCompaniesQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportCompaniesQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportCompaniesQueryHandler : IRequestHandler<ExportCompaniesQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        //private readonly IStringLocalizer<ExportProductsQueryHandler> _localizer;

        public ExportCompaniesQueryHandler(IUnitOfWork<int> unitOfWork, IExcelService excelService)
        {
            _unitOfWork = unitOfWork;
            _excelService = excelService;
        }

        public async Task<Result<string>> Handle(ExportCompaniesQuery request, CancellationToken cancellationToken)
        {
            var companyFilterSpec = new CompanyFilterSpecification(request.SearchString);
            var products = await _unitOfWork.Repository<Company>().Entities
                .Specify(companyFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(products, mappers: new Dictionary<string, Func<Company, object>>
            {
                { "Id", item => item.Id },
                { "Name", item => item.Name },
                { "ShortTitle", item => item.ShortTitle },
                { "RegistrationNo", item => item.RegistrationNo },
                { "RegistrationDate", item => DateTime.SpecifyKind((DateTime)item.RegistrationDate, DateTimeKind.Utc).ToLocalTime().ToString("G", CultureInfo.CurrentCulture) }
            }, sheetName: "Companies");

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
