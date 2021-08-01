using MediatR;
using Microsoft.EntityFrameworkCore;
using RuS.Application.Extensions;
using RuS.Application.Interfaces.Repositories;
using RuS.Application.Specifications.Core;
using RuS.Domain.Entities.Core;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RuS.Application.Features.Sites.Queries.GetAllPaged
{
    public class GetAllSitesQuery : IRequest<PaginatedResult<GetAllPagedSitesResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public string[] OrderBy { get; set; }

        public GetAllSitesQuery(int pageNumber, int pageSize, string searchString, string orderBy)
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

    internal class GetAllPagedSitesQueryHandler : IRequestHandler<GetAllSitesQuery, PaginatedResult<GetAllPagedSitesResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllPagedSitesQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPagedSitesResponse>> Handle(GetAllSitesQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Site, GetAllPagedSitesResponse>> expression = s => new GetAllPagedSitesResponse
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                CompanyId = s.CompanyId
            };
            var steFilterSpec = new SiteFilterSpecification(request.SearchString);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<Site>().Entities
                   .Include(s => s.Company)
                   .Specify(steFilterSpec)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<Site>().Entities
                   .Include(s => s.Company)
                   .Specify(steFilterSpec)
                   .OrderBy(ordering) // require using System.Linq.Dynamic.Core;
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
        }
    }
}
