using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using RuS.Application.Features.Categories.Commands;
using RuS.Application.Features.Categories.Queries;
using RuS.Client.Infrastructure.Managers.Project.Category;
using RuS.Shared.Constants.Application;
using RuS.Shared.Constants.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RuS.Client.Pages.Project.Category
{
    public partial class Categories
    {
        [Inject] private ICategoryManager CategoryManager { get; set; }

        [Parameter] public int Id { get; set; }
        [Parameter] public AddEditCategoryCommand _command { get; set; } = new();
        [Parameter] public CategoryResponse _category { get; set; } = new();
        private List<CategoryResponse> _categoryList = new();

        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateCategories;
        private bool _canEditCategories;
        private bool _canDeleteCategories;
        private bool _canExportCategories;
        private bool _canSearchCategories;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateCategories = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Categories.Create)).Succeeded;
            _canEditCategories = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Categories.Edit)).Succeeded;
            _canDeleteCategories = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Categories.Delete)).Succeeded;
            _canExportCategories = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Categories.Export)).Succeeded;
            _canSearchCategories = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Categories.Search)).Succeeded;

            await GetCategoriesAsync();
            _loaded = true;
        }

        private async Task GetCategoriesAsync()
        {
            var response = await CategoryManager.GetAllAsync();
            if (response.Succeeded)
            {
                _categoryList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private async Task SaveAsync()
        {
            var response = await CategoryManager.SaveAsync(_command);
            if (response.Succeeded)
            {
                await Reset();
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

        private async Task Delete(int id)
        {
            string deleteContent = _localizer["Delete Category"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await CategoryManager.DeleteAsync(id);
                if (response.Succeeded)
                {
                    await Reset();
                    _snackBar.Add(response.Messages[0], Severity.Success);
                }
                else
                {
                    await Reset();
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }

        private async Task ExportToExcel()
        {
            var response = await CategoryManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(Categories).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Categories exported"]
                    : _localizer["Filtered Categories exported"], Severity.Success);
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private async Task UpdateCategory(int id = 0)
        {           
            _category = _categoryList.FirstOrDefault(c => c.Id == id);
            if (_category != null)
            {
                _command.Id = _category.Id;
                _command.Name = _category.Name;
                _command.Description = _category.Description;
            }          
        }

        private async Task Reset()
        {
            _command = new();
            await GetCategoriesAsync();
        }

        private bool Search(CategoryResponse category)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (category.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (category.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
    }
}
