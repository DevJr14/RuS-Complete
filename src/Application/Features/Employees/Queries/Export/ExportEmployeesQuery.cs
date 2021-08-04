using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RuS.Application.Extensions;
using RuS.Application.Interfaces.Repositories;
using RuS.Application.Interfaces.Services;
using RuS.Application.Specifications.Core;
using RuS.Domain.Entities.Catalog;
using RuS.Domain.Entities.Core;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RuS.Application.Features.Employees.Queries.Export
{
    public class ExportEmployeesQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportEmployeesQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportEmployeesQueryHandler : IRequestHandler<ExportEmployeesQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportEmployeesQueryHandler> _localizer;

        public ExportEmployeesQueryHandler(IUnitOfWork<int> unitOfWork, IExcelService excelService, IStringLocalizer<ExportEmployeesQueryHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _excelService = excelService;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportEmployeesQuery request, CancellationToken cancellationToken)
        {
            var employeeFilterSpec = new EmployeeFilterSpecification(request.SearchString);
            var employees = await _unitOfWork.Repository<Employee>().Entities
                .Specify(employeeFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(employees, mappers: new Dictionary<string, Func<Employee, object>> 
            {

                {_localizer["Firstname"], item => item.FirstName },
                {_localizer["LastName"], item => item.LastName },
                {_localizer["CompanyName"], item => item.Company.Name },
                {_localizer["EmployeeNo"], item => item.EmployeeNo },
                {_localizer["MiddleName"], item => item.MiddleName },
                {_localizer["Gender"], item => item.Gender },
                {_localizer["DateOfBirth"], item => DateTime.SpecifyKind((DateTime)item.DateOfBirth, DateTimeKind.Utc).ToLocalTime().ToString("G", CultureInfo.CurrentCulture)  },
                {_localizer["CellphoneNo"], item => item.CellphoneNo },
                {_localizer["Email"], item => item.Email },
                {_localizer["Street"], item => item.Street },
                {_localizer["Suburb"], item => item.Suburb },
                {_localizer["City"], item => item.City },
                {_localizer["Postcode"], item => item.Postcode }

            }, sheetName: _localizer["Employees"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
