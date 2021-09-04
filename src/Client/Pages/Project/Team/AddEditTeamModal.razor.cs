using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using RuS.Application.Features.Employees.Queries.GetById;
using RuS.Application.Features.Teams.Commands.AddEdit;
using RuS.Client.Infrastructure.Managers.Core.Employee;
using RuS.Client.Infrastructure.Managers.Project.Team;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RuS.Client.Pages.Project.Team
{
    public partial class AddEditTeamModal
    {
        [Inject] private ITeamManager TeamManager { get; set; }
        [Inject] public IEmployeeManager EmployeeManager { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        [Parameter] public int Id { get; set; }
        [Parameter] public AddEditTeamCommand _command { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private List<GetEmployeeResponse> _employees = new();
        public HashSet<string> SelectedMembers { get; set; } = new HashSet<string>();

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            await LoadEmployees();
        }

        private async Task LoadEmployees()
        {
            var response = await EmployeeManager.GetAllNotPaged();
            if (response.Succeeded)
            {
                _employees = response.Data;
            }
        }

        private async Task SaveAsync()
        {
            var response = await TeamManager.SaveAsync(_command);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
                await AddTeamMembers(response.Data);
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

        private async Task AddTeamMembers(int teamId)
        {
            var command = new AddRemoveTeamMemberCommand()
            {
                TeamId = teamId,
                Members = SelectedMembers
            };
            var response = await TeamManager.SaveMemberAsync(command);
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

        private async Task<IEnumerable<int>> SearchEmployees(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _employees.Select(x => x.Id);

            return _employees.Where(x => x.FirstName.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }
        public void Cancel()
        {
            MudDialog.Cancel();
        }
    }
}
