using Microsoft.AspNetCore.Components;
using MudBlazor;
using RuS.Application.Features.Projects.Queries;
using RuS.Client.Infrastructure.Managers.Project.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RuS.Client.Pages.Project.Project
{
    public partial class ProjectList
    {
        [Inject] private IProjectManager ProjectManager { get; set; }
        public List<ProjectResponse> _projectList { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            await GetProjectsAsync();
        }

        private async Task GetProjectsAsync()
        {
            var response = await ProjectManager.GetAllAsync();
            if (response.Succeeded)
            {
                _projectList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }        
    }
}
