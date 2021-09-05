using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using RuS.Application.Features.Teams.Commands.AddEdit;
using RuS.Application.Features.Teams.Queries;
using RuS.Client.Infrastructure.Managers.Project.Team;
using RuS.Shared.Constants.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RuS.Client.Pages.Project.Team
{
    public partial class ProjectTeams
    {
        [Inject] public ITeamManager TeamManager { get; set; }

        [Parameter]
        public int Id { get; set; }

        private string _searchString = "";

        private List<TeamResponse> _teamList = new();

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            var data = await TeamManager.GetAllForProjectAsync(Id);
            if (data.Succeeded)
            {
                _teamList = data.Data;
            }
        }
        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                var team = _teamList.FirstOrDefault(t => t.Id == id);
                if (team != null)
                {
                    parameters.Add(nameof(AddEditTeamModal._command), new AddEditTeamCommand
                    {
                        Id = team.Id,
                        ProjectId = Id,                   
                        Name = team.Name,
                        TeamLeaderId = team.TeamLeaderId,                        
                        Description = team.Description
                    });
                }
            }
            else
            {
                parameters.Add(nameof(AddEditTeamModal._command), new AddEditTeamCommand
                {
                    ProjectId = Id
                });
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditTeamModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await LoadData();
            }
        }

        private async Task InvokeMembersModal(int id)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {

                var team = _teamList.FirstOrDefault(t => t.Id == id);
                if (team != null)
                {
                    parameters.Add(nameof(Id), id);
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<ManageMembers>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await LoadData();
            }

        }
        
        private async Task Delete(int id)
        {
            string deleteContent = _localizer["Delete Project Team"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await TeamManager.DeleteAsync(id);
                if (response.Succeeded)
                {
                    _snackBar.Add(response.Messages[0], Severity.Success);
                    await LoadData();
                }
                else
                {
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }

        private async Task ExportToExcel()
        {
            var response = await TeamManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(ProjectTeams).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Project Teams exported"]
                    : _localizer["Filtered Project Teams exported"], Severity.Success);
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
    }
}
