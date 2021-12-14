using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using RuS.Application.Features.Tasks.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RuS.Client.Pages.Project.Tasks
{
    public partial class TaskCard
    {
        [Parameter]
        public TaskResponse Task { get; set; }

        [Parameter] public EventCallback<int> OnEditSelection { get; set; }
        [Parameter] public EventCallback<int> OnDeleteSelection { get; set; }

        protected async Task DeleteSelectionChanged(MouseEventArgs e, int tastId)
        {
            await OnDeleteSelection.InvokeAsync(tastId);
        }

        protected async Task EditTaskSelectionChanged(MouseEventArgs e, int id)
        {
            await OnEditSelection.InvokeAsync(id);
        }

        private void Details(int id)
        {
            _navigationManager.NavigateTo($"/tasks/details/{id}");
        }
    }
}
