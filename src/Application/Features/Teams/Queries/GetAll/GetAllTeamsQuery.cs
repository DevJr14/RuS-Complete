using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RuS.Application.Interfaces.Repositories;
using RuS.Domain.Entities.Projects;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RuS.Application.Features.Teams.Queries.GetAll
{
    public class GetAllTeamsQuery : IRequest<Result<List<TeamResponse>>>
    {
    }

    internal class GetAllTeamsQueryHandler : IRequestHandler<GetAllTeamsQuery, Result<List<TeamResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetAllTeamsQueryHandler> _localizer;

        public GetAllTeamsQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<GetAllTeamsQueryHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<List<TeamResponse>>> Handle(GetAllTeamsQuery request, CancellationToken cancellationToken)
        {
            var teams = await _unitOfWork.Repository<Team>().Entities
                 .Include(t => t.Project)
                 .ToListAsync();
            if (teams.Count > 0)
            {
                var mappedTeams = _mapper.Map<List<TeamResponse>>(teams);
                return await Result<List<TeamResponse>>.SuccessAsync(mappedTeams);
            }
            else
            {
                return await Result<List<TeamResponse>>.FailAsync(_localizer["No Teams."]);
            }
        }
    }
}
