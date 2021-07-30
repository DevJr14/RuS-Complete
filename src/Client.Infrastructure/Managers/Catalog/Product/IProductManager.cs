using RuS.Application.Features.Products.Commands.AddEdit;
using RuS.Application.Features.Products.Queries.GetAllPaged;
using RuS.Application.Requests.Catalog;
using RuS.Shared.Wrapper;
using System.Threading.Tasks;

namespace RuS.Client.Infrastructure.Managers.Catalog.Product
{
    public interface IProductManager : IManager
    {
        Task<PaginatedResult<GetAllPagedProductsResponse>> GetProductsAsync(GetAllPagedProductsRequest request);

        Task<IResult<string>> GetProductImageAsync(int id);

        Task<IResult<int>> SaveAsync(AddEditProductCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}