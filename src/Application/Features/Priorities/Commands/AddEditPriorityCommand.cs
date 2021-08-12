using AutoMapper;
using MediatR;
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

namespace RuS.Application.Features.Priorities.Commands
{
    public class AddEditPriorityCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    internal class AddEditPriorityCommandHandler : IRequestHandler<AddEditPriorityCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IPriorityRepository _priorityRepository;
        private readonly IStringLocalizer<AddEditPriorityCommandHandler> _localizer;

        public AddEditPriorityCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IPriorityRepository priorityRepository, IStringLocalizer<AddEditPriorityCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _priorityRepository = priorityRepository;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditPriorityCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var notUnique = await _priorityRepository.IsUnique(command.Name);
                if (notUnique)
                {
                    return await Result<int>.FailAsync(_localizer["Priority already exists."]);
                }
                else
                {
                    var priority = _mapper.Map<Priority>(command);
                    await _unitOfWork.Repository<Priority>().AddAsync(priority);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(priority.Id, _localizer["Priority Saved"]);
                }
            }
            else
            {
                var notUnique = await _priorityRepository.IsUnique(command.Name, command.Id);
                if (notUnique)
                {
                    return await Result<int>.FailAsync(_localizer["Priority already exists."]);
                }
                else
                {
                    var priority = await _unitOfWork.Repository<Priority>().GetByIdAsync(command.Id);
                    if (priority != null)
                    {
                        priority.Name = command.Name ?? priority.Name;
                        priority.Description = command.Description ?? priority.Description;
                        await _unitOfWork.Repository<Priority>().UpdateAsync(priority);
                        await _unitOfWork.Commit(cancellationToken);
                        return await Result<int>.SuccessAsync(priority.Id, _localizer["Priority Updated"]);
                    }
                    else
                    {
                        return await Result<int>.FailAsync(_localizer["Priority Not Found!"]);
                    }
                }
            }
        }
    }
}
