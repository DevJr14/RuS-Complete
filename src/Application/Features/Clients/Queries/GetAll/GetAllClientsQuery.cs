using AutoMapper;
using MediatR;
using RuS.Application.Features.Statuses.Queries;
using RuS.Application.Interfaces.Repositories;
using RuS.Domain.Entities.Projects;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RuS.Application.Features.Clients.Queries.GetAll
{
    public class GetAllClientsQuery : IRequest<Result<List<ClientResponse>>>
    {
    }

    internal class GetAllClientsQueryHandler : IRequestHandler<GetAllClientsQuery, Result<List<ClientResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllClientsQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<ClientResponse>>> Handle(GetAllClientsQuery request, CancellationToken cancellationToken)
        {
            var clients = await _unitOfWork.Repository<Client>().GetAllAsync();
            if (clients.Count > 0)
            {
                var mappedClients = _mapper.Map<List<ClientResponse>>(clients);
                return await Result<List<ClientResponse>>.SuccessAsync(mappedClients);
            }
            else
            {
                return new Result<List<ClientResponse>>();
            }
        }
    }
}
