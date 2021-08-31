using Microsoft.AspNetCore.Components;
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
        [Parameter] public int MyProperty { get; set; }

        private void Details()
        {
            _navigationManager.NavigateTo("/project/projects/add");
        }
    }
}
