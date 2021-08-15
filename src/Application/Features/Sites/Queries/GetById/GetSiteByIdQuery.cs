using AutoMapper;
using MediatR;
using RuS.Application.Interfaces.Repositories;
using RuS.Domain.Entities.Core;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RuS.Application.Features.Sites.Queries.GetById
{
    public class GetSiteByIdQuery : IRequest<Result<SiteResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetSiteByIdQueryHandler : IRequestHandler<GetSiteByIdQuery, Result<SiteResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetSiteByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<SiteResponse>> Handle(GetSiteByIdQuery request, CancellationToken cancellationToken)
        {
            var site = await _unitOfWork.Repository<Site>().GetByIdAsync(request.Id);
            site.Company = await _unitOfWork.Repository<Company>().GetByIdAsync(site.CompanyId);
            var mappedSite = _mapper.Map<SiteResponse>(site);
            return await Result<SiteResponse>.SuccessAsync(mappedSite);
        }
    }
}
