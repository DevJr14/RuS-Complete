using AutoMapper;
using MediatR;
using RuS.Application.Interfaces.Repositories;
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
    public class GetAllCategoriesQuery : IRequest<Result<List<CategoryResponse>>>
    {
    }

    internal class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, Result<List<CategoryResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllCategoriesQueryHandler(IMapper mapper, IUnitOfWork<int> unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<CategoryResponse>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _unitOfWork.Repository<Category>().GetAllAsync();
            if (categories.Count > 0)
            {
                var mappedCategory = _mapper.Map<List<CategoryResponse>>(categories);
                return await Result<List<CategoryResponse>>.SuccessAsync(mappedCategory);
            }
            else
            {
                return await Result<List<CategoryResponse>>.FailAsync("No Categories");
            }
        }
    }
}
