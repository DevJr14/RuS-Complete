using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using RuS.Application.Interfaces.Repositories;
using RuS.Application.Mappings;
using RuS.Domain.Entities.Projects;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace RuS.Application.Features.Teams.Commands.AddEdit
{
    public class AddEditTeamCommand : IRequest<Result<int>>, IMapFrom<Team>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int ProjectId { get; set; }

        public string Description { get; set; }

        public int TeamLeaderId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AddEditTeamCommand, Team>();
        }
    }

    internal class AddEditTeamCommandHandler : IRequestHandler<AddEditTeamCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditTeamCommandHandler> _localizer;
        private readonly ITeamRepository _teamRepository;

        public AddEditTeamCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditTeamCommandHandler> localizer, ITeamRepository teamRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
            _teamRepository = teamRepository;
        }

        public async Task<Result<int>> Handle(AddEditTeamCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var notUnique = await _teamRepository.IsUniqueEntry(command);
                if (notUnique)
                {
                    return await Result<int>.FailAsync(_localizer["Team already exists."]);
                }
                else
                {
                    var team = _mapper.Map<Team>(command);
                    await _unitOfWork.Repository<Team>().AddAsync(team);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(team.Id, _localizer["Team Saved"]);
                }
            }
            else
            {
                var notUnique = await _teamRepository.IsUniqueEntry(command);
                if (notUnique)
                {
                    return await Result<int>.FailAsync(_localizer["Team already exists."]);
                }
                else
                {
                    var team = await _unitOfWork.Repository<Team>().GetByIdAsync(command.Id);
                    if (team != null)
                    {
                        team.Name = command.Name ?? team.Name;
                        team.Description = command.Description ?? team.Description;
                        team.ProjectId = command.ProjectId;
                        team.TeamLeaderId = command.TeamLeaderId;
                        await _unitOfWork.Repository<Team>().UpdateAsync(team);
                        await _unitOfWork.Commit(cancellationToken);
                        return await Result<int>.SuccessAsync(team.Id, _localizer["Team Updated"]);
                    }
                    else
                    {
                        return await Result<int>.FailAsync(_localizer["Team Not Found!"]);
                    }
                }
            }
        }
    }
}
