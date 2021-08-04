using MediatR;
using RuS.Application.Extensions;
using RuS.Application.Features.Products.Queries.GetAllPaged;
using RuS.Application.Interfaces.Repositories;
using RuS.Application.Specifications.Catalog;
using RuS.Application.Specifications.Core;
using RuS.Domain.Entities.Catalog;
using RuS.Domain.Entities.Core;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RuS.Application.Features.Employees.Queries.GetAllPaged
{
    public class GetAllEmployeesQuery : IRequest<PaginatedResult<GetAllPagedEmployeesResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public string[] OrderBy { get; set; }

        public GetAllEmployeesQuery(int pageNumber, int pageSize, string searchString, string orderBy)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                OrderBy = orderBy.Split(',');
            }
        }
    }

    internal class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, PaginatedResult<GetAllPagedEmployeesResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllEmployeesQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPagedEmployeesResponse>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Employee, GetAllPagedEmployeesResponse>> expression = e => new GetAllPagedEmployeesResponse
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                MiddleName = e.MiddleName,
                CompanyId = e.CompanyId,
                CompanyName = e.Company.Name,
                DateOfBirth = e.DateOfBirth,
                Gender = e.Gender,
                EmployeeNo = e.EmployeeNo,
                ImageUrl = e.ImageUrl,
                Street = e.Street,
                Suburb = e.Suburb,
                City = e.City,
                Postcode = e.Postcode,
                Email = e.Email,
                CellphoneNo = e.CellphoneNo
            };

            var employeeFilterSpec = new EmployeeFilterSpecification(request.SearchString);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<Employee>().Entities
                   .Specify(employeeFilterSpec)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<Employee>().Entities
                   .Specify(employeeFilterSpec)
                   .OrderBy(ordering) // require System.Linq.Dynamic.Core
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}
