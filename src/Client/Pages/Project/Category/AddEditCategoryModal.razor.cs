using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using RuS.Application.Features.Categories.Commands;
using RuS.Client.Infrastructure.Managers.Catalog.Brand;
using RuS.Client.Infrastructure.Managers.Project.Category;
using RuS.Client.Pages.Catalog;
using RuS.Shared.Constants.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RuS.Client.Pages.Project.Category
{
    public partial class AddEditCategoryModal
    {
        [Inject] private ICategoryManager CategoryManager { get; set; }
        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        [Parameter] public int Id { get; set; }
        [Parameter] public AddEditCategoryCommand _command { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            var response = await CategoryManager.SaveAsync(_command);
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
