using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using RuS.Application.Features.Clients.Commands.AddEdit;
using RuS.Application.Features.Clients.Queries;
using RuS.Client.Infrastructure.Managers.Project.Client;
using System.Threading.Tasks;

namespace RuS.Client.Pages.Project.Client
{
    public partial class AddEditClient
    {
        [Inject] public IClientManager ClientManager { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        [Parameter] public int Id { get; set; }
        [Parameter] public AddEditClientCommand _command { get; set; } = new();
        [Parameter] public ClientResponse _client { get; set; }

        private async Task SaveAsync()
        {
            var response = await ClientManager.SaveAsync(_command);
            if (response.Succeeded)
            {
                Cancel();
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

        protected override async Task OnInitializedAsync()
        {
            if (Id != 0)
            {
                var response = await ClientManager.GetByIdAsync(Id);
                if (response.Succeeded)
                {
                    _client = response.Data;

                    _command.Id = _client.Id;
                    _command.Name = _client.Name;
                    _command.ContactPerson = _client.ContactPerson;
                    _command.CellphoneNo = _client.CellphoneNo;
                    _command.TelephoneNo = _client.TelephoneNo;
                    _command.Street = _client.Street;
                    _command.Suburb = _client.Suburb;
                    _command.City = _client.City;
                    _command.Postcode = _client.Postcode;
                    _command.Email = _client.Email;
                }
            }
        }

        private void Cancel()
        {
            _navigationManager.NavigateTo($"/project/clients/");
        }
    }
}
