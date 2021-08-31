using Microsoft.AspNetCore.Components;
using RuS.Application.Features.Tasks.Queries;
using RuS.Client.Infrastructure.Managers.Project.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RuS.Client.Pages.Project.Tasks
{
    public partial class ProjectTasks
    {
        [Inject] public ITaskManager TaskManager { get; set; }

        [Parameter]
        public int Id { get; set; }

        private List<TaskResponse> _taskList = new();

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            var data = await TaskManager.GetAllForProjectAsync(Id);
            if (data.Succeeded)
            {
                _taskList = data.Data;
            }
        }
    }
}
