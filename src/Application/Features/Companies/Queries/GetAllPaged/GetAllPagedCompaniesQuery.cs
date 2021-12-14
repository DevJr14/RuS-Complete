using MediatR;
using RuS.Application.Extensions;
using RuS.Application.Interfaces.Repositories;
using RuS.Application.Specifications.Core;
using RuS.Domain.Entities.Core;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RuS.Application.Features.Companies.Queries.GetAllPaged
{
    public class GetAllPagedCompaniesQuery : IRequest<PaginatedResult<GetAllPagedCompaniesResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public string[] OrderBy { get; set; }

        public GetAllPagedCompaniesQuery(int pageNumber, int pageSize, string searchString, string orderBy)
        {
            SearchString = searchString;
            PageSize = pageSize;
            PageNumber = pageNumber;
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                OrderBy = orderBy.Split(',');
            }
        }
    }

    internal class GetAllCompaniesQueryHandler : IRequestHandler<GetAllPagedCompaniesQuery, PaginatedResult<GetAllPagedCompaniesResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllCompaniesQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPagedCompaniesResponse>> Handle(GetAllPagedCompaniesQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Company, GetAllPagedCompaniesResponse>> expression = e => new GetAllPagedCompaniesResponse
            {
                Id = e.Id,
                Name = e.Name,
                ShortTitle = e.ShortTitle,
                RegistrationNo = e.RegistrationNo,
                RegistrationDate = e.RegistrationDate
            };
            var companyFilterSpec = new CompanyFilterSpecification(request.SearchString);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<Company>().Entities
                   //.Specify(companyFilterSpec)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<Company>().Entities
                   .Specify(companyFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
        }
    }
}
