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

namespace RuS.Application.Features.Tasks.Queries.GetById
{
    public class GetTaskByIdQuery : IRequest<Result<TaskResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, Result<TaskResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetTaskByIdQueryHandler> _localizer;

        public GetTaskByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<GetTaskByIdQueryHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<TaskResponse>> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            var task = await _unitOfWork.Repository<Task>().Entities
                .Where(t => t.Id == request.Id)
                .Include(t => t.Priority)
                .Include(t => t.Project)
                .Include(t => t.Status)
                .Include(t => t.Category)
                .FirstOrDefaultAsync();
            if (task != null)
            {
                var mappedTask = _mapper.Map<TaskResponse>(task);
                return await Result<TaskResponse>.SuccessAsync(mappedTask);
            }
            else
            {
                return await Result<TaskResponse>.FailAsync(_localizer[$"Task with Id: {request.Id} Not Found"]);
            }
        }
    }
}
