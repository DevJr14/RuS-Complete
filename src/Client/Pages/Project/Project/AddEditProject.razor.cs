using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using RuS.Application.Features.Categories.Commands;
using RuS.Application.Features.Categories.Queries;
using RuS.Application.Features.Clients.Queries;
using RuS.Application.Features.Priorities.Queries;
using RuS.Application.Features.Projects.Commands.AddEdit;
using RuS.Application.Features.Projects.Queries;
using RuS.Application.Features.Sites.Queries.GetById;
using RuS.Application.Features.Statuses.Queries;
using RuS.Application.Features.Tasks.Commands.AddEdit;
using RuS.Application.Features.Tasks.Queries;
using RuS.Application.Features.Teams.Queries;
using RuS.Client.Infrastructure.Managers.Core.Site;
using RuS.Client.Infrastructure.Managers.Project.Category;
using RuS.Client.Infrastructure.Managers.Project.Client;
using RuS.Client.Infrastructure.Managers.Project.Priority;
using RuS.Client.Infrastructure.Managers.Project.Project;
using RuS.Client.Infrastructure.Managers.Project.Status;
using RuS.Client.Infrastructure.Managers.Project.Task;
using RuS.Client.Infrastructure.Managers.Project.Team;
using RuS.Client.Pages.Project.Category;
using RuS.Client.Pages.Project.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RuS.Client.Pages.Project.Project
{
    public partial class AddEditProject
    {
        [Inject] public IProjectManager ProjectManager { get; set; }
        [Inject] public ISiteManager SiteManager { get; set; }
        [Inject] public IClientManager ClientManager { get; set; }
        [Inject] public ICategoryManager CategoryManager { get; set; }
        [Inject] public IPriorityManager PriorityManager { get; set; }
        [Inject] public IStatusManager StatusManager { get; set; }
        [Inject] public ITaskManager TaskManager { get; set; }
        [Inject] public ITeamManager TeamManager { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        [Parameter] public int Id { get; set; }
        [Parameter] public AddEditProjectCommand _command { get; set; } = new();
        [Parameter] public ProjectResponse _project { get; set; }

        private List<TaskResponse> _taskList = new();
        private List<TeamResponse> _teamkList = new();

        private List<SiteResponse> _sites = new();
        private List<ClientResponse> _clients = new();
        private List<StatusResponse> _statuses = new();
        private List<PriorityResponse> _priorities = new();
        private List<CategoryResponse> _categories = new();

        MudDatePicker _picker;
        private bool autoClose = true;

        private async Task SaveAsync()
        {
            var response = await ProjectManager.SaveAsync(_command);
            if (response.Succeeded)
            {
                Cancel();
                _snackBar.Add(response.Messages[0], Severity.Success);
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            if (Id != 0)
            {
                var response = await ProjectManager.GetByIdAsync(Id);
                if (response.Succeeded)
                {
                    _project = response.Data;

                    _command.Id = _project.Id;
                    _command.Name = _project.Name;
                    _command.SiteId = _project.SiteId;
                    _command.CategoryId = (int)_project.CategoryId;
                    _command.ClientId = _project.ClientId;
                    _command.PriorityId = (int)_project.PriorityId;
                    _command.StatusId = (int)_project.StatusId;
                    _command.Description = _project.Description;
                    _command.ScopeOfWork = _project.ScopeOfWork;
                    _command.Start = _project.Start;
                    _command.End = _project.End;
                    _command.ActualStart = _project.ActualStart;
                    _command.ActualEnd = _project.ActualEnd;
                }
            }
            await LoadDataAsync();
        }       

        private async Task LoadDataAsync()
        {
            await LoadTasks();
            await LoadTeams();
            await LoadSitesAsync();
            await LoadCategories();
            await LoadPriorities();
            await LoadStatuses();
            await LoadClients();
        }

        private async Task LoadTasks()
        {
            var data = await TaskManager.GetAllForProjectAsync(Id);
            if (data.Succeeded)
            {
                _taskList = data.Data;
            }
        }

        private async Task LoadTeams()
        {
            var data = await TeamManager.GetAllForProjectAsync(Id);
            if (data.Succeeded)
            {
                _teamkList = data.Data;
            }
        }

        private async Task LoadSitesAsync()
        {
            var data = await SiteManager.GetAllSitesAsync();
            if (data.Succeeded)
            {
                _sites = data.Data;
            }
        }

        private async Task LoadCategories()
        {
            var response = await CategoryManager.GetAllAsync();
            if (response.Succeeded)
            {
                _categories = response.Data;
            }
        }

        private async Task LoadStatuses()
        {
            var response = await StatusManager.GetAllAsync();
            if (response.Succeeded)
            {
                _statuses = response.Data;
            }
        }

        private async Task LoadPriorities()
        {
            var response = await PriorityManager.GetAllAsync();
            if (response.Succeeded)
            {
                _priorities = response.Data;
            }
        }

        private async Task LoadClients()
        {
            var response = await ClientManager.GetAllAsync();
            if (response.Succeeded)
            {
                _clients = response.Data;
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
                await Reset();
            }
        }

        private async Task Reset()
        {
            await LoadTasks();
        }

        private async Task<IEnumerable<int>> SearchSites(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _sites.Select(x => x.Id);

            return _sites.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }

        private async Task<IEnumerable<int>> SearchClients(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _clients.Select(x => x.Id);

            return _clients.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }

        private async Task<IEnumerable<int>> SearchCategories(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _categories.Select(x => x.Id);

            return _categories.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }

        private async Task<IEnumerable<int>> SearchStatuses(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _statuses.Select(x => x.Id);

            return _statuses.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }

        private async Task<IEnumerable<int>> SearchPriorities(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _priorities.Select(x => x.Id);

            return _priorities.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }

        private void Cancel()
        {
            _navigationManager.NavigateTo($"/project/projects/");
        }
    }
}
