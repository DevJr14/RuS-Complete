using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using RuS.Application.Features.Tasks.Commands.AddEdit;
using RuS.Application.Features.Tasks.Queries;
using RuS.Client.Infrastructure.Managers.Project.Project;
using RuS.Client.Infrastructure.Managers.Project.Task;
using RuS.Shared.Constants.Application;
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

        private string _searchString = "";

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

        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                var task = _taskList.FirstOrDefault(t => t.Id == id);
                if (task != null)
                {
                    parameters.Add(nameof(AddEditTaskModal._command), new AddEditTaskCommand
                    {
                        Id = task.Id,
                        Name = task.Name,
                        Description = task.Description,
                        CategoryId = (int)task.CategoryId,
                        PriorityId = (int)task.PriorityId,
                        ProjectId = Id,
                        StatusId = (int)task.StatusId,
                        Start = task.Start,
                        End = task.End
                    });
                }
            }
            else
            {
                parameters.Add(nameof(AddEditTaskModal._command), new AddEditTaskCommand
                {
                    ProjectId = Id
                });
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditTaskModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await LoadData();
            }
        }

        private async Task Delete(int id)
        {
            string deleteContent = _localizer["Delete Project Task"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await TaskManager.DeleteAsync(id);
                if (response.Succeeded)
                {
                    _snackBar.Add(response.Messages[0], Severity.Success);
                    await LoadData();
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

        private async Task ExportToExcel()
        {
            var response = await TaskManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(ProjectTasks).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Project Tasks exported"]
                    : _localizer["Filtered Project Tasks exported"], Severity.Success);
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
