using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RuS.Application.Extensions;
using RuS.Application.Interfaces.Repositories;
using RuS.Application.Interfaces.Services;
using RuS.Application.Specifications.Core;
using RuS.Application.Specifications.Project;
using RuS.Domain.Entities.Core;
using RuS.Domain.Entities.Projects;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RuS.Application.Features.Clients.Queries.Export
{
    public class ExportClientsQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportClientsQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportClientsQueryHandler : IRequestHandler<ExportClientsQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportClientsQueryHandler> _localizer;

        public ExportClientsQueryHandler(IUnitOfWork<int> unitOfWork, IExcelService excelService, IStringLocalizer<ExportClientsQueryHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _excelService = excelService;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportClientsQuery request, CancellationToken cancellationToken)
        {
            var clientFilterSpec = new ClientFilterSpecification(request.SearchString);
            var clients = await _unitOfWork.Repository<Client>().Entities
                .Specify(clientFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(clients, mappers: new Dictionary<string, Func<Client, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Name"], item => item.Name },
                { _localizer["ContactPerson"], item => item.ContactPerson },
                { _localizer["TelephoneNo"], item => item.TelephoneNo },
                { _localizer["CellphoneNo"], item => item.CellphoneNo },
                { _localizer["Email"], item => item.Email },
                { _localizer["Street"], item => item.Street },
                { _localizer["Suburb"], item => item.Suburb },
                { _localizer["City"], item => item.City },
                { _localizer["Postcode"], item => item.Postcode },
            }, sheetName: _localizer["Clients"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
