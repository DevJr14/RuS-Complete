using AutoMapper;
using LazyCache;
using MediatR;
using RuS.Application.Interfaces.Repositories;
using RuS.Domain.Entities.Core;
using RuS.Shared.Constants.Application;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RuS.Application.Features.Companies.Queries.GetAll
{
    public class GetAllCompaniesQuery : IRequest<Result<List<GetAllCompaniesResponse>>>
    {
        //public GetAllCompaniesQuery()
        //{

        //}
    }

    internal class GetAllCompaniesQueryHandler : IRequestHandler<GetAllCompaniesQuery, Result<List<GetAllCompaniesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllCompaniesQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllCompaniesResponse>>> Handle(GetAllCompaniesQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<Company>>> getAllCompanies = () => _unitOfWork.Repository<Company>().GetAllAsync();
            var companyList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllCompaniesCacheKey, getAllCompanies);
            var mappedCompanies = _mapper.Map<List<GetAllCompaniesResponse>>(companyList);
            return await Result<List<GetAllCompaniesResponse>>.SuccessAsync(mappedCompanies);
        }
    }
}
