using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using RuS.Application.Features.Teams.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RuS.Client.Pages.Project.Team
{
    public partial class TeamCard
    {
        [Parameter]
        public TeamResponse Team { get; set; }

        [Parameter] public EventCallback<int> OnEditSelection { get; set; }
        [Parameter] public EventCallback<int> OnDeleteSelection { get; set; }
        [Parameter] public EventCallback<int> OnMembersSelection { get; set; }
        protected async Task DeleteSelectionChanged(MouseEventArgs e, int teamId)
        {
            await OnDeleteSelection.InvokeAsync(teamId);
        }

        protected async Task EditTeamSelectionChanged(MouseEventArgs e, int id)
        {
            await OnEditSelection.InvokeAsync(id);
        }

        protected async Task ManageTeamMembers(MouseEventArgs e, int teamId)
        {
            await OnMembersSelection.InvokeAsync(teamId);
        }
    }
}
