using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RuS.Application.Interfaces.Repositories;
using RuS.Domain.Entities.Projects;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RuS.Application.Features.Categories.Commands
{
    public class AddEditCategoryCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }

    internal class AddEditCategoryCommandHandler : IRequestHandler<AddEditCategoryCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IStringLocalizer<AddEditCategoryCommandHandler> _localizer;

        public AddEditCategoryCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ICategoryRepository categoryRepository, IStringLocalizer<AddEditCategoryCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditCategoryCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var notUnique = !await _categoryRepository.IsUniqueEntry(command.Name);
                if (notUnique)
                {
                    return await Result<int>.FailAsync(_localizer["Category already exists."]);
                }
                else
                {
                    var category = _mapper.Map<Category>(command);
                    await _unitOfWork.Repository<Category>().AddAsync(category);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(category.Id, _localizer["Category Saved"]);
                }
            }
            else
            {
                var notUnique = !await _categoryRepository.IsUniqueEntry(command.Name, command.Id);
                if (notUnique)
                {
                    return await Result<int>.FailAsync(_localizer["Category already exists."]);
                }
                var category = await _unitOfWork.Repository<Category>().GetByIdAsync(command.Id);
                if (category != null)
                {
                    category.Name = command.Name;
                    category.Description = command.Description??category.Description;
                    await _unitOfWork.Repository<Category>().UpdateAsync(category);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(category.Id, _localizer["Category Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Category Not Found!"]);
                }
            }
        }
    }
}
