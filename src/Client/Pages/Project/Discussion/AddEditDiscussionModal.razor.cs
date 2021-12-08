using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using RuS.Application.Features.Discussions.Commands.AddEdit;
using RuS.Client.Infrastructure.Managers.Project.Discussion;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RuS.Client.Pages.Project.Discussion
{
    public partial class AddEditDiscussionModal
    {
        [Inject] public IDiscussionManager DiscussionManager { get; set; }
        [Parameter] public AddEditDiscussionCommand _command { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        private async Task SaveAsync()
        {
            var response = await DiscussionManager.SaveAsync(_command);
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

        public void Cancel()
        {
            MudDialog.Cancel();
        }
    }
}
