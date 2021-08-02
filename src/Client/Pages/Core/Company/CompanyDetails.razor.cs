using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using RuS.Application.Features.Companies.Queries.GetById;
using RuS.Application.Features.Sites.Commands.AddEdit;
using RuS.Application.Features.Sites.Queries.GetAllPaged;
using RuS.Application.Features.Sites.Queries.GetById;
using RuS.Client.Extensions;
using RuS.Client.Infrastructure.Managers.Core.Company;
using RuS.Client.Infrastructure.Managers.Core.Site;
using RuS.Shared.Constants.Application;
using RuS.Shared.Constants.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RuS.Client.Pages.Core.Company
{
    public partial class CompanyDetails
    {
        [Parameter] public int Id { get; set; }
        [Inject] private ICompanyManager CompanyManager { get; set; }
        [Inject] private ISiteManager SiteManager { get; set; }
        
        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private GetCompanyResponse _company;
        private GetSiteResponse _site;

        [Parameter] public string Name { get; set; }
        [Parameter] public string ShortTitle { get; set; }
        [Parameter] public string RegistrationNo { get; set; }
        [Parameter] public DateTime? RegistrationDate { get; set; }

        private string _searchString = "";
        private bool _dense = true;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateSites;
        private bool _canEditCompanies;
        private bool _canEditSites;
        private bool _canDeleteSites;
        private bool _canExportSites;
        private bool _canSearchSites;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateSites = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Sites.Create)).Succeeded;
            _canEditCompanies = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Companies.Edit)).Succeeded;
            _canEditSites = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Sites.Edit)).Succeeded;
            _canDeleteSites = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Sites.Delete)).Succeeded;
            _canExportSites = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Sites.Export)).Succeeded;
            _canSearchSites = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Sites.Search)).Succeeded;

            await LoadData();
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task LoadData()
        {
            var response = await CompanyManager.GetByIdAsync(Id);
            if (response.Succeeded)
            {
                _company = response.Data;
                Name = _company.Name;
                ShortTitle = _company.ShortTitle;
                RegistrationNo = _company.RegistrationNo;
                RegistrationDate = _company.RegistrationDate;
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
                _site = _company.Sites.FirstOrDefault(c => c.Id == id);
                if (_site != null)
                {
                    parameters.Add(nameof(AddEditSiteModal.Command), new AddEditSiteCommand
                    {
                        Id = _site.Id,
                        Name = _site.Name,
                        Description = _site.Description,
                        CompanyId = _site.CompanyId
                    });
                }
            }
            else
            {
                parameters.Add(nameof(AddEditSiteModal.Command), new AddEditSiteCommand
                {
                    CompanyId = Id
                });
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditSiteModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task Delete(int id)
        {
            string deleteContent = _localizer["Delete Content"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await SiteManager.DeleteAsync(id);
                if (response.Succeeded)
                {
                    await Reset();
                    await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
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
            var response = await SiteManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"Sites_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Sites exported"]
                    : _localizer["Filtered Sites exported"], Severity.Success);
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private async Task Reset()
        {
            //Load only sites with companyId
            await LoadData();
        }

        private bool Search(GetSiteResponse site)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (site.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (site.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
    }
}
