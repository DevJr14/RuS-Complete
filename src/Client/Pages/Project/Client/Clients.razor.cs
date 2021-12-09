using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using RuS.Application.Features.Clients.Queries;
using RuS.Client.Infrastructure.Managers.Project.Client;
using RuS.Shared.Constants.Application;
using RuS.Shared.Constants.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RuS.Client.Pages.Project.Client
{
    public partial class Clients
    {
        [Inject] private IClientManager ClientManager { get; set; }
        public List<ClientResponse> _clientList { get; set; } = new();
        public ClientResponse _client { get; set; } = new();

        private string _searchString = "";
        private bool _dense = true;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateClients;
        private bool _canEditClients;
        private bool _canDeleteClients;
        private bool _canExportClients;
        private bool _canSearchClients;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateClients = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Clients.Create)).Succeeded;
            _canEditClients = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Clients.Edit)).Succeeded;
            _canDeleteClients = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Clients.Delete)).Succeeded;
            _canExportClients = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Clients.Export)).Succeeded;
            _canSearchClients = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Clients.Search)).Succeeded;

            await GetClientsAsync();
            _loaded = true;
        }

        private async Task GetClientsAsync()
        {
            var response = await ClientManager.GetAllAsync();
            if (response.Succeeded)
            {
                _clientList = response.Data.ToList();
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
            string deleteContent = _localizer["Delete Client"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await ClientManager.DeleteAsync(id);
                if (response.Succeeded)
                {
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
        }

        private async Task ExportToExcel()
        {
            var response = await ClientManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(Clients).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Clients exported"]
                    : _localizer["Filtered Clients exported"], Severity.Success);
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private void ClientDetails(int id)
        {
            if (id == 0)
            {
                _navigationManager.NavigateTo("/project/clients/add");
            }
            else
            {
                _navigationManager.NavigateTo($"/project/clients/edit/{id}");
            }
        }

        private async Task Reset()
        {
            await GetClientsAsync();
        }

        private bool Search(ClientResponse client)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (client.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (client.Street?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (client.Suburb?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }

        private void Cancel()
        {
            _navigationManager.NavigateTo("/");
        }
    }
}
