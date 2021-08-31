using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using RuS.Application.Features.Categories.Commands;
using RuS.Application.Features.Categories.Queries;
using RuS.Application.Features.Priorities.Queries;
using RuS.Application.Features.Statuses.Queries;
using RuS.Application.Features.Tasks.Commands.AddEdit;
using RuS.Client.Infrastructure.Managers.Core.Site;
using RuS.Client.Infrastructure.Managers.Project.Category;
using RuS.Client.Infrastructure.Managers.Project.Priority;
using RuS.Client.Infrastructure.Managers.Project.Status;
using RuS.Client.Infrastructure.Managers.Project.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RuS.Client.Pages.Project.Tasks
{
public partial class AddEditTaskModal
{
        [Inject] private ITaskManager TaskManager { get; set; }
        [Inject] public ICategoryManager CategoryManager { get; set; }
        [Inject] public IPriorityManager PriorityManager { get; set; }
        [Inject] public IStatusManager StatusManager { get; set; }
        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        [Parameter] public int Id { get; set; }
        [Parameter] public AddEditTaskCommand _command { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private List<StatusResponse> _statuses = new();
        private List<PriorityResponse> _priorities = new();
        private List<CategoryResponse> _categories = new();

        MudDatePicker _picker;
        private bool autoClose = true;

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            var response = await TaskManager.SaveAsync(_command);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
                MudDialog.Close();
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
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            await LoadCategories();
            await LoadPriorities();
            await LoadStatuses();
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

    }
}
