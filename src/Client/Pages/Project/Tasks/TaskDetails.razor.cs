using Microsoft.AspNetCore.Components;
using MudBlazor;
using RuS.Application.Features.Discussions.Commands.AddEdit;
using RuS.Application.Features.Discussions.Queries;
using RuS.Application.Features.Tasks.Queries;
using RuS.Client.Infrastructure.Managers.Project.Discussion;
using RuS.Client.Infrastructure.Managers.Project.Task;
using RuS.Client.Pages.Project.Discussion;
using RuS.Client.Shared.Dialogs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RuS.Client.Pages.Project.Tasks
{
    public partial class TaskDetails
    {
        [Inject] public ITaskManager TaskManager { get; set; }
        [Inject] public IDiscussionManager DiscussionManager {get;set;}

        [Parameter] public int Id { get; set; }
        public TaskResponse _task { get; set; } = new();

        public List<DiscussionResponse> _discussions { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            await LoadTask();
            await LoadDiscussions();            
        }

        private async Task LoadTask()
        {
            var response = await TaskManager.GetByIdAsync(Id);
            if (response.Succeeded)
            {
                _task = response.Data;
            }
        }

        private async Task LoadDiscussions()
        {
            var response = await DiscussionManager.GetAllForTaskAsync(Id);
            if (response.Succeeded)
            {
                _discussions = response.Data;
            }
        }

        private async Task InvokeDelete(int id)
        {
            string deleteContent = _localizer["Delete Task Comment"];
            var parameters = new DialogParameters
            {
                {nameof(DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await DiscussionManager.DeleteAsync(id);
                if (response.Succeeded)
                {
                    _snackBar.Add(response.Messages[0], Severity.Success);
                    await LoadDiscussions();
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

        private async Task InvokeAdd(int projectId, int? taskId) 
        {
            var parameters = new DialogParameters();
            parameters.Add(nameof(AddEditDiscussionModal._command), new AddEditDiscussionCommand
            {
                ProjectId = projectId,
                TaskId = taskId
            });

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditDiscussionModal>(_localizer["Edit Comment"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await LoadDiscussions();
            }
        }

        private async Task InvokeEdit(int id) 
        {
            var parameters = new DialogParameters();
            var discussion = _discussions.FirstOrDefault(d => d.Id == id);
            if(discussion != null)
            {
                parameters.Add(nameof(AddEditDiscussionModal._command), new AddEditDiscussionCommand
                {
                    Id = discussion.Id,
                    ProjectId = discussion.ProjectId,
                    TaskId = discussion.TaskId,
                    Comment = discussion.Comment
                });
            }

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditDiscussionModal>(_localizer["Edit Comment"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await LoadDiscussions();
            }
        }

        private void Cancel()
        {
            _navigationManager.NavigateTo($"/project/projects/edit/{Id}");
        }

    }
}
