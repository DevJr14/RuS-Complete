using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RuS.Application.Features.Tasks.Queries.GetById;
using RuS.Application.Interfaces.Repositories;
using RuS.Domain.Entities.Projects;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RuS.Application.Features.Teams.Queries.GetById
{
    public class GetTeamByIdQuery : IRequest<Result<TeamResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetTeamByIdQueryHandler : IRequestHandler<GetTeamByIdQuery, Result<TeamResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetTeamByIdQueryHandler> _localizer;

        public GetTeamByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<GetTeamByIdQueryHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<TeamResponse>> Handle(GetTeamByIdQuery request, CancellationToken cancellationToken)
        {
            var team = await _unitOfWork.Repository<Team>().Entities
                 .Include(t => t.Project)
                 .Where(t => t.Id == request.Id)
                 .FirstOrDefaultAsync();
            if (team != null)
            {
                var mappedTeam = _mapper.Map<TeamResponse>(team);
                return await Result<TeamResponse>.SuccessAsync(mappedTeam);
            }
            else
            {
                return await Result<TeamResponse>.FailAsync(_localizer[$"Team with Id: {request.Id} Not Found"]);
            }
        }
    }
}
