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

namespace RuS.Application.Features.Statuses.Queries
{
    public class GetAllStatusesQuery : IRequest<Result<List<StatusResponse>>>
    {
    }

    internal class GetAllStatusesQueryHandler : IRequestHandler<GetAllStatusesQuery, Result<List<StatusResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllStatusesQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<StatusResponse>>> Handle(GetAllStatusesQuery request, CancellationToken cancellationToken)
        {
            var status = await _unitOfWork.Repository<Status>().GetAllAsync();
            if (status.Count > 0)
            {
                var mappedStatus = _mapper.Map<List<StatusResponse>>(status);
                return await Result<List<StatusResponse>>.SuccessAsync(mappedStatus);
            }
            else
            {
                return await Result<List<StatusResponse>>.FailAsync("No Statuses");
            }
        }
    }
}
