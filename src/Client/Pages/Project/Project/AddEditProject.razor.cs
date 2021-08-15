using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using RuS.Application.Features.Categories.Queries;
using RuS.Application.Features.Clients.Queries;
using RuS.Application.Features.Priorities.Queries;
using RuS.Application.Features.Projects.Commands.AddEdit;
using RuS.Application.Features.Projects.Queries;
using RuS.Application.Features.Sites.Queries.GetById;
using RuS.Application.Features.Statuses.Queries;
using RuS.Client.Infrastructure.Managers.Core.Site;
using RuS.Client.Infrastructure.Managers.Project.Category;
using RuS.Client.Infrastructure.Managers.Project.Client;
using RuS.Client.Infrastructure.Managers.Project.Priority;
using RuS.Client.Infrastructure.Managers.Project.Project;
using RuS.Client.Infrastructure.Managers.Project.Status;
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

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        [Parameter] public int Id { get; set; }
        [Parameter] public AddEditProjectCommand _command { get; set; } = new();
        [Parameter] public ProjectResponse _project { get; set; }

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
                    _command.CategoryId = _project.CategoryId;
                    _command.ClientId = _project.ClientId;
                    _command.PriorityId = _project.PriorityId;
                    _command.StatusId = _project.StatusId;
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
            await LoadSitesAsync();
            await LoadCategories();
            await LoadPriorities();
            await LoadStatuses();
            await LoadClients();
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
