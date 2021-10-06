using RuS.Application.Features.Categories.Commands;
using RuS.Application.Features.Categories.Queries;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Client.Infrastructure.Managers.Project.Category
{
    public interface ICategoryManager : IManager
    {
        Task<IResult<List<CategoryResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditCategoryCommand command);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");

        Task<IResult<CategoryResponse>> GetByIdAsync(int id);
    }
}
