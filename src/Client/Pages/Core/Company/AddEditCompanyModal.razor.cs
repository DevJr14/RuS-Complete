using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using RuS.Application.Features.Companies.Commands.AddEdit;
using RuS.Client.Extensions;
using RuS.Client.Infrastructure.Managers.Core.Company;
using RuS.Shared.Constants.Application;
using System.Threading.Tasks;

namespace RuS.Client.Pages.Core.Company
{
    public partial class AddEditCompanyModal
    {
        [Inject] private ICompanyManager CompanyManager { get; set; }

        [Parameter] public AddEditCompanyCommand AddEditCompanyModel { get; set; } = new();
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            var response = await CompanyManager.SaveAsync(AddEditCompanyModel);
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
            //await LoadDataAsync();
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        //private async Task LoadDataAsync()
        //{

        //}
    }
}
