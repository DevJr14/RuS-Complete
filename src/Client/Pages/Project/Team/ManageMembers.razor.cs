using Microsoft.AspNetCore.Components;
using MudBlazor;
using RuS.Application.Features.Teams.Commands.AddEdit;
using RuS.Application.Responses.Identity;
using RuS.Application.Responses.Project.Team;
using RuS.Client.Infrastructure.Managers.Project.Team;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RuS.Client.Pages.Project.Team
{
    public partial class ManageMembers
    {
        [Inject] public ITeamManager TeamManager { get; set; }

        [Parameter]
        public int TeamId { get; set; }
        [Parameter] public string TeamName { get; set; }
        [Parameter] public string Description { get; set; }

        public List<TeamMemberModel> TeamMembers { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        public TeamMemberModel _teamMember = new();
        private bool _loaded;

        private string _searchString = "";
        protected override async Task OnInitializedAsync()
        {
            var teamResult = await TeamManager.GetByIdAsync(TeamId);
            if (teamResult.Succeeded)
            {
                var team = teamResult.Data;
                if(team != null)
                {
                    TeamName = team.Name;
                    Description = "Add or Remove Member(s)";
                    var response = await TeamManager.GetTeamMembers(team.Id);
                    if (response.Succeeded)
                    {
                        TeamMembers = response.Data.TeamMembers;
                    }
                }
            }
            _loaded = true;
        }

        private async Task SaveAsync()
        {
            var command = new UpdateTeamMembersCommand()
            {
                TeamId = TeamId,
                TeamMembers = TeamMembers
            };
            var response = await TeamManager.UpdateTeamMembersAsync(command);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
                MudDialog.Close();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        public bool Search(TeamMemberModel teamMember)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (teamMember.Firstname?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (teamMember.Lastname?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
    }
}
