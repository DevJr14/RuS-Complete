using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using RuS.Application.Features.Companies.Queries.GetAll;
using RuS.Application.Features.Companies.Queries.GetAllPaged;
using RuS.Application.Features.Employees.Commands.AddEdit;
using RuS.Application.Features.Employees.Queries.GetById;
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
        [Parameter] public GetEmployeeResponse Employee { get; set; } = new();
        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllCompaniesResponse> _companies = new();

        private async Task SaveAsync()
        {
            var response = await EmployeeManager.SaveAsync(EmployeeCommand);
            if (response.Succeeded)
            {
                Cancel();
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
            if (Id != 0)
            {
                var response = await EmployeeManager.GetByIdAsync(Id);
                if (response.Succeeded)
                {
                    Employee = response.Data;
                    EmployeeCommand.Id = Employee.Id;
                    EmployeeCommand.FirstName = Employee.FirstName;
                    EmployeeCommand.LastName = Employee.LastName;
                    EmployeeCommand.MiddleName = Employee.MiddleName;
                    EmployeeCommand.Gender = Employee.Gender;
                    EmployeeCommand.CompanyId = Employee.CompanyId;
                    EmployeeCommand.CellphoneNo = Employee.CellphoneNo;
                    EmployeeCommand.Email = Employee.Email;
                    EmployeeCommand.Street = Employee.Street;
                    EmployeeCommand.Suburb = Employee.Suburb;
                    EmployeeCommand.City = Employee.City;
                    EmployeeCommand.DateOfBirth = Employee.DateOfBirth;
                    EmployeeCommand.Postcode = Employee.Postcode;
                    EmployeeCommand.ImageUrl = Employee.ImageUrl;
                }
            }
            await LoadDataAsync();
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task LoadDataAsync()
        {
            await LoadCompaniesAsync();
        }

        private async Task LoadCompaniesAsync()
        {
            var data = await CompanyManger.GetAllCompaniesAsync();
            if (data.Succeeded)
            {
                _companies = data.Data;
            }
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

        private async Task<IEnumerable<int>> SearchCompany(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _companies.Select(x => x.Id);

            return _companies.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }

        private void Cancel()
        {
            _navigationManager.NavigateTo($"/core/employees/");            
        }
    }
}
