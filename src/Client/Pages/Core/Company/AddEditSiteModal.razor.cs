using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using RuS.Application.Features.Companies.Queries.GetById;
using RuS.Application.Features.Sites.Commands.AddEdit;
using RuS.Client.Extensions;
using RuS.Client.Infrastructure.Managers.Core.Company;
using RuS.Client.Infrastructure.Managers.Core.Site;
using RuS.Shared.Constants.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RuS.Client.Pages.Core.Company
{
    public partial class AddEditSiteModal
    {
        [Parameter] public AddEditSiteCommand Command { get; set; } = new();
        [Inject] private ICompanyManager CompanyManager { get; set; }
        [Inject] private ISiteManager SiteManager { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private GetCompanyResponse _company;

        [Parameter] public string CompanyName { get; set; }

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            Command.CompanyId = _company.Id;
            var response = await SiteManager.SaveAsync(Command);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
                await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
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
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task LoadDataAsync()
        {
            await LoadCompanyAsync();
        }

        private async Task LoadCompanyAsync()
        {
            var response = await CompanyManager.GetByIdAsync(Command.CompanyId);
            if (response.Succeeded)
            {
                _company = response.Data;
                CompanyName = _company.Name;
            }
        }

        private async Task<IEnumerable<int>> SearchSites(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _company.Sites.Select(s => s.Id);

            return _company.Sites.Where(s => s.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase) || s.Description.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }
    }
}
