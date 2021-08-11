using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RuS.Application.Extensions;
using RuS.Application.Interfaces.Repositories;
using RuS.Application.Interfaces.Services;
using RuS.Application.Specifications.Project;
using RuS.Domain.Entities.Projects;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RuS.Application.Features.Categories.Queries
{
    public class ExportCategoriesQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }
        public ExportCategoriesQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportCategoriesQueryHandler : IRequestHandler<ExportCategoriesQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportCategoriesQueryHandler> _localizer;

        public ExportCategoriesQueryHandler(IUnitOfWork<int> unitOfWork, IExcelService excelService, IStringLocalizer<ExportCategoriesQueryHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _excelService = excelService;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categoryFilterSpec = new CategoryFilterSpecification(request.SearchString);
            var category = await _unitOfWork.Repository<Category>().Entities
                .Specify(categoryFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(category, mappers: new Dictionary<string, Func<Category, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Name"], item => item.Name },
                { _localizer["Description"], item => item.Description }
            }, sheetName: _localizer["Categories"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
