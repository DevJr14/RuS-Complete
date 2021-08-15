using Blazored.FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using RuS.Application.Features.Priorities.Commands;
using RuS.Application.Features.Priorities.Queries;
using RuS.Client.Infrastructure.Managers.Project.Priority;
using RuS.Shared.Constants.Application;
using RuS.Shared.Constants.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RuS.Client.Pages.Project.Priority
{
    public partial class Priorities
    {
        [Inject] private IPriorityManager PriorityManager { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        [Parameter] public int Id { get; set; }
        [Parameter] public AddEditPriorityCommand _command { get; set; } = new();
        [Parameter] public PriorityResponse _priority { get; set; } = new();
        private List<PriorityResponse> _priorityList = new();

        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreatePriorities;
        private bool _canEditPriorities;
        private bool _canDeletePriorities;
        private bool _canExportPriorities;
        private bool _canSearchPriorities;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreatePriorities = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Priorities.Create)).Succeeded;
            _canEditPriorities = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Priorities.Edit)).Succeeded;
            _canDeletePriorities = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Priorities.Delete)).Succeeded;
            _canExportPriorities = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Priorities.Export)).Succeeded;
            _canSearchPriorities = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Priorities.Search)).Succeeded;

            await GetPrioritiesAsync();
            _loaded = true;
        }

        private async Task GetPrioritiesAsync()
        {
            var response = await PriorityManager.GetAllAsync();
            if (response.Succeeded)
            {
                _priorityList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private async Task SaveAsync()
        {
            var response = await PriorityManager.SaveAsync(_command);
            if (response.Succeeded)
            {
                await Reset();
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

        private async Task Delete(int id)
        {
            string deleteContent = _localizer["Delete Priority"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await PriorityManager.DeleteAsync(id);
                if (response.Succeeded)
                {
                    await Reset();
                    _snackBar.Add(response.Messages[0], Severity.Success);
                }
                else
                {
                    await Reset();
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }

        private async Task ExportToExcel()
        {
            var response = await PriorityManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(Priorities).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Priorities exported"]
                    : _localizer["Filtered Priorities exported"], Severity.Success);
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private async Task UpdatePriority(int id = 0)
        {
            _priority = _priorityList.FirstOrDefault(c => c.Id == id);
            if (_priority != null)
            {
                _command.Id = _priority.Id;
                _command.Name = _priority.Name;
                _command.Description = _priority.Description;
            }
        }

        private async Task Reset()
        {
            _command = new();
            await GetPrioritiesAsync();
        }

        private bool Search(PriorityResponse priority)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (priority.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (priority.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
    }
}
