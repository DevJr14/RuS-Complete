using Blazored.FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using RuS.Application.Features.Projects.Commands.AddEdit;
using RuS.Application.Features.Projects.Queries;
using RuS.Client.Infrastructure.Managers.Project.Project;
using RuS.Shared.Constants.Application;
using RuS.Shared.Constants.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RuS.Client.Pages.Project.Project
{
    public partial class Projects
    {
        [Inject] private IProjectManager ProjectManager { get; set; }
        public List<ProjectResponse> _projectList { get; set; } = new();
        public ProjectResponse _project { get; set; } = new();

        private string _searchString = "";
        private bool _dense = true;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateProjects;
        private bool _canEditProjects;
        private bool _canDeleteProjects;
        private bool _canExportProjects;
        private bool _canSearchProjects;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateProjects = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Projects.Create)).Succeeded;
            _canEditProjects = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Projects.Edit)).Succeeded;
            _canDeleteProjects = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Projects.Delete)).Succeeded;
            _canExportProjects = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Projects.Export)).Succeeded;
            _canSearchProjects = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Projects.Search)).Succeeded;

            await GetProjectsAsync();
            _loaded = true;
        }

        private async Task GetProjectsAsync()
        {
            var response = await ProjectManager.GetAllAsync();
            if (response.Succeeded)
            {
                _projectList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private async Task Delete(int id)
        {
            string deleteContent = _localizer["Delete Project"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await ProjectManager.DeleteAsync(id);
                if (response.Succeeded)
                {
                    _snackBar.Add(response.Messages[0], Severity.Success);
                    await Reset();
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
            var response = await ProjectManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(Projects).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Projects exported"]
                    : _localizer["Filtered Projects exported"], Severity.Success);
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private void AddEdit(int id)
        {
            if (id == 0)
            {
                _navigationManager.NavigateTo("/project/projects/add");
            }
            else
            {
                _navigationManager.NavigateTo($"/project/projects/edit/{id}");
            }
        }

        private void ReturnHome()
        {
            _navigationManager.NavigateTo("/");
        }

        private async Task Reset()
        {
            await GetProjectsAsync();
        }

        private bool Search(ProjectResponse project)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (project.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (project.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (project.Code?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
    }
}
