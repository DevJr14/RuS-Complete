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
    public class GetStatusByIdQuery : IRequest<Result<StatusResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetStatusByIdQueryHandler : IRequestHandler<GetStatusByIdQuery, Result<StatusResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetStatusByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<StatusResponse>> Handle(GetStatusByIdQuery request, CancellationToken cancellationToken)
        {
            var status = await _unitOfWork.Repository<Status>().GetByIdAsync(request.Id);
            if (status != null)
            {
                var mappedStatus = _mapper.Map<StatusResponse>(status);
                return await Result<StatusResponse>.SuccessAsync(mappedStatus);
            }
            else
            {
                return await Result<StatusResponse>.FailAsync("Status Not Found!");
            }
        }
    }
}
