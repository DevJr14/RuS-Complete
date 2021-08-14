using RuS.Application.Features.Sites.Commands.AddEdit;
using RuS.Application.Features.Sites.Queries.GetAllPaged;
using RuS.Application.Features.Sites.Queries.GetById;
using RuS.Application.Requests.Core;
using RuS.Client.Infrastructure.Extensions;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Client.Infrastructure.Managers.Core.Site
{
    public class SiteManager : ISiteManager
    {
        private readonly HttpClient _httpClient;

        public SiteManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.SiteEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.SiteEndpoints.Export
                : Routes.SiteEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<SiteResponse>> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync(Routes.SiteEndpoints.GetById(id));
            return await response.ToResult<SiteResponse>();
        }

        public async Task<PaginatedResult<GetAllPagedSitesResponse>> GetProductsAsync(GetAllPagedSitesRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.SiteEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllPagedSitesResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditSiteCommand command)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.SiteEndpoints.Save, command);
            return await response.ToResult<int>();
        }
    }
}
