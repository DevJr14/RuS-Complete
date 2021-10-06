using AutoMapper;
using MediatR;
using RuS.Application.Interfaces.Repositories;
using RuS.Domain.Entities.Projects;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RuS.Application.Features.Priorities.Queries
{
    public class GetAllPrioritiesQuery : IRequest<Result<List<PriorityResponse>>>
    {
    }

    internal class GetAllPrioritiesQueryHandler : IRequestHandler<GetAllPrioritiesQuery, Result<List<PriorityResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllPrioritiesQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<PriorityResponse>>> Handle(GetAllPrioritiesQuery request, CancellationToken cancellationToken)
        {
            var priorities = await _unitOfWork.Repository<Priority>().GetAllAsync();
            if (priorities.Count > 0)
            {
                var mappedPriorities = _mapper.Map<List<PriorityResponse>>(priorities);
                return await Result<List<PriorityResponse>>.SuccessAsync(mappedPriorities);
            }
            else
            {
                return await Result<List<PriorityResponse>>.FailAsync("No Priority");
            }
        }
    }
}
