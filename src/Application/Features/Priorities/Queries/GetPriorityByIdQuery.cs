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
    public class GetPriorityByIdQuery : IRequest<Result<PriorityResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetPriorityByIdQueryHandler : IRequestHandler<GetPriorityByIdQuery, Result<PriorityResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetPriorityByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<PriorityResponse>> Handle(GetPriorityByIdQuery request, CancellationToken cancellationToken)
        {
            var priority = await _unitOfWork.Repository<Priority>().GetByIdAsync(request.Id);
            if (priority != null)
            {
                var mappedPriority = _mapper.Map<PriorityResponse>(priority);
                return await Result<PriorityResponse>.SuccessAsync(mappedPriority);
            }
            else
            {
                return await Result<PriorityResponse>.FailAsync("Priority Not Found!");
            }
        }
    }
}
