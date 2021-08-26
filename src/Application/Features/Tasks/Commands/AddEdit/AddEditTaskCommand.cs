using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using RuS.Application.Interfaces.Repositories;
using RuS.Application.Mappings;
using RuS.Domain.Entities.Projects;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Task = RuS.Domain.Entities.Projects.Task;

namespace RuS.Application.Features.Tasks.Commands.AddEdit
{
    public class AddEditTaskCommand : IRequest<Result<int>>, IMapFrom<Task>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public int ProjectId { get; set; }

        public int? StatusId { get; set; }

        public int? CategoryId { get; set; }

        public int? PriorityId { get; set; }

        public string Description { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AddEditTaskCommand, Task>();
        }
    }

    internal class AddEditTaskCommandHandler : IRequestHandler<AddEditTaskCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditTaskCommandHandler> _localizer;
        private readonly ITaskRepository _taskRepository;

        public AddEditTaskCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditTaskCommandHandler> localizer, ITaskRepository taskRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
            _taskRepository = taskRepository;
        }

        public async Task<Result<int>> Handle(AddEditTaskCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var notUnique = await _taskRepository.IsUniqueEntry(command);
                if (notUnique)
                {
                    return await Result<int>.FailAsync(_localizer["Task already exists."]);
                }
                else
                {
                    var task = _mapper.Map<Task>(command);
                    task.Code = _taskRepository.GenerateProjectCode();
                    await _unitOfWork.Repository<Task>().AddAsync(task);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(task.Id, _localizer["Task Saved"]);
                }
            }
            else
            {
                var notUnique = await _taskRepository.IsUniqueEntry(command);
                if (notUnique)
                {
                    return await Result<int>.FailAsync(_localizer["Task already exists."]);
                }
                else
                {
                    var task = await _unitOfWork.Repository<Task>().GetByIdAsync(command.Id);
                    if (task != null)
                    {
                        task.Name = command.Name ?? task.Name;
                        task.Description = command.Description ?? task.Description;
                        task.StatusId = command.StatusId;
                        task.CategoryId = command.CategoryId;
                        task.PriorityId = command.PriorityId;
                        task.Start = command.Start;
                        task.End = command.End;
                        await _unitOfWork.Repository<Task>().UpdateAsync(task);
                        await _unitOfWork.Commit(cancellationToken);
                        return await Result<int>.SuccessAsync(task.Id, _localizer["Task Updated"]);
                    }
                    else
                    {
                        return await Result<int>.FailAsync(_localizer["Task Not Found!"]);
                    }
                }
            }
        }
    }
}
