using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using RuS.Application.Features.Statuses.Commands;
using RuS.Client.Infrastructure.Managers.Project.Status;
using System.Threading.Tasks;

namespace RuS.Client.Pages.Project.Status
{
    public partial class AddEditStatusModal
    {
        [Inject] private IStatusManager StatusManager { get; set; }
        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        [Parameter] public int Id { get; set; }
        [Parameter] public AddEditStatusCommand _command { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            var response = await StatusManager.SaveAsync(_command);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
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
        }

        private static async Task LoadDataAsync()
        {
            await Task.CompletedTask;
        }
    }
}
