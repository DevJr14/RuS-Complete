using RuS.Application.Features.Categories.Commands;
using RuS.Application.Features.Categories.Queries;
using RuS.Client.Infrastructure.Extensions;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Client.Infrastructure.Managers.Project.Category
{
    public class CategoryManager : ICategoryManager
    {
        private readonly HttpClient _httpClient;

        public CategoryManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.CategoryEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.CategoryEndpoints.Export
                : Routes.CategoryEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<List<CategoryResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.CategoryEndpoints.GetAll);
            return await response.ToResult<List<CategoryResponse>>();
        }

        public async Task<IResult<CategoryResponse>> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync(Routes.CategoryEndpoints.GetById(id));
            return await response.ToResult<CategoryResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditCategoryCommand command)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.CategoryEndpoints.Save, command);
            return await response.ToResult<int>();
        }
    }
}
