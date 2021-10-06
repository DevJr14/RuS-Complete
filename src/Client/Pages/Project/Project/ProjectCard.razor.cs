using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using RuS.Application.Features.Projects.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RuS.Client.Pages.Project.Project
{
    public partial class ProjectCard
    {
        [Parameter] public ProjectResponse Project { get; set; }

        [Parameter] public EventCallback<int> OnEditSelection { get; set; }
        [Parameter] public EventCallback<int> OnDeleteSelection { get; set; }

        private void Details(int id)
        {
            _navigationManager.NavigateTo($"/project/projects/edit/{id}");
        }

        protected async Task DeleteSelectionChanged(MouseEventArgs e, int projectId)
        {
            await OnDeleteSelection.InvokeAsync(projectId);
        }

        protected async Task EditProjectSelectionChanged(MouseEventArgs e, int id)
        {
            await OnEditSelection.InvokeAsync(id);           
        }
    }
}
