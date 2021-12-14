using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using RuS.Application.Features.Employees.Commands.AddEdit;
using RuS.Application.Features.Employees.Queries.GetAllPaged;
using RuS.Application.Requests.Core;
using RuS.Client.Extensions;
using RuS.Client.Infrastructure.Managers.Core.Company;
using RuS.Client.Infrastructure.Managers.Core.Employee;
using RuS.Shared.Constants.Application;
using RuS.Shared.Constants.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RuS.Client.Pages.Core.Employee
{
    public partial class Employees
    {
        [Inject] private IEmployeeManager EmployeeManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private IEnumerable<GetAllPagedEmployeesResponse> _pagedData;
        private MudTable<GetAllPagedEmployeesResponse> _table;
        private int _totalItems;
        private int _currentPage;
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateEmployees;
        private bool _canEditEmployees;
        private bool _canDeleteEmployees;
        private bool _canExportEmployees;
        private bool _canSearchEmployees;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateEmployees = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Employees.Create)).Succeeded;
            _canEditEmployees = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Employees.Edit)).Succeeded;
            _canDeleteEmployees = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Employees.Delete)).Succeeded;
            _canExportEmployees = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Employees.Export)).Succeeded;
            _canSearchEmployees = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Employees.Search)).Succeeded;

            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task<TableData<GetAllPagedEmployeesResponse>> ServerReload(TableState state)
        {
            if (!string.IsNullOrWhiteSpace(_searchString))
            {
                state.Page = 0;
            }
            await LoadData(state.Page, state.PageSize, state);
            return new TableData<GetAllPagedEmployeesResponse> { TotalItems = _totalItems, Items = _pagedData };
        }

        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
            string[] orderings = null;
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                orderings = state.SortDirection != SortDirection.None ? new[] { $"{state.SortLabel} {state.SortDirection}" } : new[] { $"{state.SortLabel}" };
            }

            var request = new GetAllPagedEmployeesRequest { PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, Orderby = orderings };
            var response = await EmployeeManager.GetEmployeesAsync(request);
            if (response.Succeeded)
            {
                _totalItems = response.TotalCount;
                _currentPage = response.CurrentPage;
                _pagedData = response.Data;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private void OnSearch(string text)
        {
            _searchString = text;
            _table.ReloadServerData();
        }

        private async Task ExportToExcel()
        {
            var response = await EmployeeManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(Employees).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Employees exported"]
                    : _localizer["Filtered Employees exported"], Severity.Success);
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private void EmployeeDetails(int id)
        {
            if(id == 0)
            {
                _navigationManager.NavigateTo($"/core/employees/add/");
            }
            else
            {
                _navigationManager.NavigateTo($"/core/employees/edit/{id}");
            }
        }

        private async Task Delete(int id)
        {
            string deleteContent = _localizer["Delete Employee"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await EmployeeManager.DeleteAsync(id);
                if (response.Succeeded)
                {
                    OnSearch("");
                    await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
                    _snackBar.Add(response.Messages[0], Severity.Success);
                }
                else
                {
                    OnSearch("");
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }

        private void Cancel()
        {
            _navigationManager.NavigateTo("/");
        }
    }
}
