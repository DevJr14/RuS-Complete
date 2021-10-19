using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RuS.Application.Interfaces.Repositories;
using RuS.Application.Responses.Project.Team;
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
    public class UpdateTeamMembersCommand : IRequest<Result<int>>
    {
        public int TeamId { get; set; }
        public List<TeamMemberModel> TeamMembers { get; set; } = new();
    }

    internal class UpdateTeamMembersCommandHandler : IRequestHandler<UpdateTeamMembersCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<UpdateTeamMembersCommandHandler> _localizer;

        public UpdateTeamMembersCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<UpdateTeamMembersCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(UpdateTeamMembersCommand command, CancellationToken cancellationToken)
        {
            List<int> selectedMembersIds = new();

            var teamMembers = await _unitOfWork.Repository<TeamMember>().Entities
                    .Where(tm => tm.TeamId == command.TeamId)
                    .ToListAsync();

            if(teamMembers.Any())
            {
                await _unitOfWork.Repository<TeamMember>().DeleteRangeAsync(teamMembers);
                await _unitOfWork.Commit(cancellationToken);
            }

            var selectedMembers = command.TeamMembers.Where(m => m.IsSelected).ToList();

            if (selectedMembers.Any())
            {
                List<Employee> employees = await _unitOfWork.Repository<Employee>().Entities.ToListAsync();
                foreach (var member in selectedMembers)
                {
                    int memberId = employees
                        .Where(e => string.Equals(e.FirstName, member.Firstname, StringComparison.OrdinalIgnoreCase)
                                 && string.Equals(e.LastName, member.Lastname, StringComparison.OrdinalIgnoreCase))
                        .Select(e => e.Id).FirstOrDefault();

                    if (memberId > 0)
                    {
                        selectedMembersIds.Add(memberId);
                    }
                }

                if (selectedMembersIds.Any())
                {
                    var teamMemberList = new List<TeamMember>();
                    foreach (var id in selectedMembersIds)
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
                else
                {
                    return await Result<int>.FailAsync(_localizer["No Member(s) Found For Team."]);
                }
            }
            else
            {
                return await Result<int>.SuccessAsync(0, _localizer["Member(s) Removed From Team."]);
            }
        }
    }
}
