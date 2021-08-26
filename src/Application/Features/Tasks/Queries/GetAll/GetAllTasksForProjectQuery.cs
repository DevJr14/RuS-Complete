using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RuS.Application.Features.Projects.Queries;
using RuS.Application.Interfaces.Repositories;
using RuS.Domain.Entities.Projects;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static RuS.Shared.Constants.Permission.Permissions;
using Task = RuS.Domain.Entities.Projects.Task;

namespace RuS.Application.Features.Tasks.Queries.GetAll
{
    public class GetAllTasksForProjectQuery : IRequest<Result<List<TaskResponse>>>
    {
        public int ProjectId { get; set; }
    }

    internal class GetAllTasksForProjectQueryHandler : IRequestHandler<GetAllTasksForProjectQuery, Result<List<TaskResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetAllTasksForProjectQueryHandler> _localizer;

        public GetAllTasksForProjectQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<GetAllTasksForProjectQueryHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<List<TaskResponse>>> Handle(GetAllTasksForProjectQuery request, CancellationToken cancellationToken)
        {
            var tasts = await _unitOfWork.Repository<Task>().Entities
                .Where(t => t.ProjectId == request.ProjectId)
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
