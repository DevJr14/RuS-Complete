using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using RuS.Application.Features.Companies.Queries.GetAllPaged;
using RuS.Application.Features.Employees.Commands.AddEdit;
using RuS.Application.Requests;
using RuS.Client.Extensions;
using RuS.Client.Infrastructure.Managers.Core.Company;
using RuS.Client.Infrastructure.Managers.Core.Employee;
using RuS.Shared.Constants.Application;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RuS.Client.Pages.Core.Employee
{
    public partial class AddEditEmployee
    {
        [Inject] public IEmployeeManager EmployeeManager { get; set; }
        [Inject] public ICompanyManager CompanyManger { get; set; }

        [Parameter] public int Id { get; set; }
        [Parameter] public AddEditEmployeeCommand EmployeeCommand { get; set; } = new();
        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllPagedCompaniesResponse> _companies = new();

        private async Task SaveAsync()
        {
            var response = await EmployeeManager.SaveAsync(EmployeeCommand);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
                await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
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
            await LoadImageAsync();
            await LoadCompaniesAsync();
        }

        private async Task LoadCompaniesAsync()
        {
        }

        private async Task LoadImageAsync()
        {
        }

        private IBrowserFile _file;

        private void DeleteAsync()
        {
            EmployeeCommand.ImageUrl = null;
            EmployeeCommand.UploadRequest = new UploadRequest();
        }

        private async Task UploadFiles(InputFileChangeEventArgs e)
        {
            _file = e.File;
            if (_file != null)
            {
                var extension = Path.GetExtension(_file.Name);
                var format = "image/png";
                var imageFile = await e.File.RequestImageFileAsync(format, 400, 400);
                var buffer = new byte[imageFile.Size];
                await imageFile.OpenReadStream().ReadAsync(buffer);
                EmployeeCommand.ImageUrl = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                EmployeeCommand.UploadRequest = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.Employee, Extension = extension };
            }
        }

        private async Task<IEnumerable<int>> SearchBrands(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _companies.Select(x => x.Id);

            return _companies.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }

        private void ReturnToEmployees()
        {
            _navigationManager.NavigateTo($"/core/employees/");            
        }
    }
}
