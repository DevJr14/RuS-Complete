using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RuS.Application.Features.Sites.Queries.GetById;
using RuS.Application.Interfaces.Repositories;
using RuS.Domain.Entities.Core;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RuS.Application.Features.Sites.Queries.GetAll
{
    public class GetAllSitesQuery : IRequest<Result<List<SiteResponse>>>
    {
    }

    internal class GetAllSitesQueryHandler : IRequestHandler<GetAllSitesQuery, Result<List<SiteResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetAllSitesQueryHandler> _localizer;

        public GetAllSitesQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<GetAllSitesQueryHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<List<SiteResponse>>> Handle(GetAllSitesQuery request, CancellationToken cancellationToken)
        {
            var sites = await _unitOfWork.Repository<Site>().Entities.Include(s => s.Company).ToListAsync();
            if (sites.Count > 0)
            {
                var mappedSites = _mapper.Map<List<SiteResponse>>(sites);
                return await Result<List<SiteResponse>>.SuccessAsync(mappedSites);
            }
            else
            {
                return await Result<List<SiteResponse>>.FailAsync(_localizer["No Sites."]);
            }
        }
    }
}
