using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RuS.Application.Interfaces.Repositories;
using RuS.Domain.Entities.Core;
using RuS.Domain.Entities.Projects;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RuS.Application.Features.Teams.Commands.AddEdit
{
    public class AddRemoveTeamMemberCommand : IRequest<Result<int>>
    {
        public int TeamId { get; set; }
        public IEnumerable<string> Members { get; set; }
    }

    internal class AddRemoveTeamMemberCommandHandler : IRequestHandler<AddRemoveTeamMemberCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<AddRemoveTeamMemberCommandHandler> _localizer;

        public AddRemoveTeamMemberCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<AddRemoveTeamMemberCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddRemoveTeamMemberCommand command, CancellationToken cancellationToken)
        {
            List<int> membersIds = new();
            var teamMembers = await _unitOfWork.Repository<TeamMember>().Entities
                    .Where(tm => tm.TeamId == command.TeamId)
                    .ToListAsync();

            if (teamMembers.Count == 0)
            {
                foreach (var member in command.Members)
                {
                    int memberId = await _unitOfWork.Repository<Employee>().Entities
                        .Where(e => string.Equals(e.FirstName, member, StringComparison.OrdinalIgnoreCase))
                        .Select(e => e.Id)
                        .FirstOrDefaultAsync();
                    if (memberId > 0)
                    {
                        membersIds.Add(memberId);
                    }
                }

                if (membersIds.Any())
                {
                    var teamMemberList = new List<TeamMember>();
                    foreach (var id in membersIds)
                    {
                        teamMemberList.Add(new TeamMember
                        {
                            TeamId = command.TeamId,
                            EmployeeId = id
                        });
                    }
                    await _unitOfWork.Repository<TeamMember>().AddRangeAsync(teamMemberList);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(teamMemberList[0].Id, _localizer["Member(s) Added To Team."]);
                }
                return await Result<int>.SuccessAsync(0, _localizer["No Member(s) Added To Team."]);
            }
            else
            {
                foreach (var member in teamMembers)
                {
                    await _unitOfWork.Repository<TeamMember>().DeleteAsync(member);
                    await _unitOfWork.Commit(cancellationToken);
                }

                foreach (var member in command.Members)
                {
                    int memberId = await _unitOfWork.Repository<Employee>().Entities
                        .Where(e => string.Equals(e.FirstName, member, StringComparison.OrdinalIgnoreCase))
                        .Select(e => e.Id)
                        .FirstOrDefaultAsync();
                    if (memberId > 0)
                    {
                        membersIds.Add(memberId);
                    }
                }

                if (membersIds.Any())
                {
                    var teamMemberList = new List<TeamMember>();
                    foreach (var id in membersIds)
                    {
                        teamMemberList.Add(new TeamMember
                        {
                            TeamId = command.TeamId,
                            EmployeeId = id
                        });
                    }
                    await _unitOfWork.Repository<TeamMember>().AddRangeAsync(teamMemberList);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(teamMemberList[0].Id, _localizer["Team Member(s) Updated."]);
                }
                else
                {
                    return await Result<int>.SuccessAsync(0, _localizer["Member(s) Removed From Team."]);
                }
            }
        }
    }
}
