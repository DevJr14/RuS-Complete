using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RuS.Application.Interfaces.Repositories;
using RuS.Domain.Entities.Projects;
using RuS.Shared.Wrapper;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RuS.Application.Features.Clients.Queries.GetById
{
    public class GetClientByIdQuery : IRequest<Result<ClientResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetClientByIdQueryHandler : IRequestHandler<GetClientByIdQuery, Result<ClientResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<GetClientByIdQueryHandler> _localizer;

        public GetClientByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<GetClientByIdQueryHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<ClientResponse>> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
        {
            var client = await _unitOfWork.Repository<Client>().Entities
                .Where(c => c.Id == request.Id)
                .FirstOrDefaultAsync();
            if (client != null)
            {
                var mappedClient = _mapper.Map<ClientResponse>(client);
                return await Result<ClientResponse>.SuccessAsync(mappedClient);
            }
            return await Result<ClientResponse>.FailAsync(_localizer[$"Client With Id: {request.Id} Not Found!"]);
        }
    }
}
