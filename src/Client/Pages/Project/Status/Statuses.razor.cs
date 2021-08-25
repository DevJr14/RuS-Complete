using Blazored.FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using RuS.Application.Features.Statuses.Commands;
using RuS.Application.Features.Statuses.Queries;
using RuS.Client.Infrastructure.Managers.Project.Status;
using RuS.Shared.Constants.Application;
using RuS.Shared.Constants.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RuS.Client.Pages.Project.Status
{
    public partial class Statuses
    {
        [Inject] private IStatusManager StatusManager { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        [Parameter] public int Id { get; set; }
        [Parameter] public AddEditStatusCommand _command { get; set; } = new();
        [Parameter] public StatusResponse _status { get; set; } = new();
        private List<StatusResponse> _statusList = new();

        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateStatuses;
        private bool _canEditStatuses;
        private bool _canDeleteStatuses;
        private bool _canExportStatuses;
        private bool _canSearchStatuses;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateStatuses = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Statuses.Create)).Succeeded;
            _canEditStatuses = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Statuses.Edit)).Succeeded;
            _canDeleteStatuses = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Statuses.Delete)).Succeeded;
            _canExportStatuses = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Statuses.Export)).Succeeded;
            _canSearchStatuses = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Statuses.Search)).Succeeded;

            await GetStatusesAsync();
            _loaded = true;
        }

        private async Task GetStatusesAsync()
        {
            var response = await StatusManager.GetAllAsync();
            if (response.Succeeded)
            {
                _statusList = response.Data.ToList();
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
            var response = await StatusManager.SaveAsync(_command);
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
            string deleteContent = _localizer["Delete Status"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await StatusManager.DeleteAsync(id);
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
            var response = await StatusManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(Statuses).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Statuses exported"]
                    : _localizer["Filtered Statuses exported"], Severity.Success);
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                var status = _statusList.FirstOrDefault(c => c.Id == id);
                if (status != null)
                {
                    parameters.Add(nameof(AddEditStatusModal._command), new AddEditStatusCommand
                    {
                        Id = status.Id,
                        Name = status.Name,
                        Description = status.Description
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditStatusModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task Reset()
        {
            _command = new();
            await GetStatusesAsync();
        }

        private bool Search(StatusResponse status)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (status.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (status.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
    }
}
