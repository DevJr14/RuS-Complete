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
using Task = RuS.Domain.Entities.Projects.Task;

namespace RuS.Application.Features.Tasks.Queries.GetAll
{
    public class GetAllTasksQuery : IRequest<Result<List<TaskResponse>>>
    {
    }

    internal class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, Result<List<TaskResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetAllTasksQueryHandler> _localizer;

        public GetAllTasksQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<GetAllTasksQueryHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }
        public async Task<Result<List<TaskResponse>>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {
            var tasts = await _unitOfWork.Repository<Task>().Entities
                .Include(t => t.Priority)
                .Include(t => t.Project)
                .Include(t => t.Status)
                .Include(t => t.Category)
                .ToListAsync();
            if (tasts.Count > 0)
            {
                var mappedTasks = _mapper.Map<List<TaskResponse>>(tasts);
                return await Result<List<TaskResponse>>.SuccessAsync(mappedTasks);
            }
            else
            {
                return await Result<List<TaskResponse>>.FailAsync(_localizer["No Tasks."]);
            }
        }
    }
}
