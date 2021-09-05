using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RuS.Application.Interfaces.Repositories;
using RuS.Application.Responses.Project.Team;
using RuS.Domain.Entities.Core;
using RuS.Domain.Entities.Projects;
using RuS.Shared.Wrapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RuS.Application.Features.Teams.Queries.GetAll
{
    public class GetTeamMembersQuery : IRequest<Result<TeamMembersResponse>>
    {
        public int TeamId { get; set; }
    }

    internal class GetTeamMembersQueryHandler : IRequestHandler<GetTeamMembersQuery, Result<TeamMembersResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<GetTeamMembersQueryHandler> _localizer;

        public GetTeamMembersQueryHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<GetTeamMembersQueryHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<TeamMembersResponse>> Handle(GetTeamMembersQuery request, CancellationToken cancellationToken)
        {
            var viewModel = new List<TeamMemberModel>();
            var members = await _unitOfWork.Repository<Employee>().GetAllAsync();
            foreach (var member in members)
            {
                var teamMemberModel = new TeamMemberModel
                {
                    Firstname = member.FirstName,
                    Lastname = member.LastName
                };
                if(await _unitOfWork.Repository<TeamMember>().Entities.Where(tm => tm.EmployeeId == member.Id && tm.TeamId == request.TeamId).AnyAsync())
                {
                    teamMemberModel.IsSelected = true;
                }
                else
                {
                    teamMemberModel.IsSelected = false;
                }
                viewModel.Add(teamMemberModel);
            }
            var result = new TeamMembersResponse
            {
                TeamMembers = viewModel
            };
            return await Result<TeamMembersResponse>.SuccessAsync(result);
        }
    }
}
