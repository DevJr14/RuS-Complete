using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RuS.Application.Interfaces.Repositories;
using RuS.Domain.Entities.Projects;
using RuS.Shared.Wrapper;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RuS.Application.Features.Projects.Queries.GetById
{
    public class GetProjectByIdQuery : IRequest<Result<ProjectResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, Result<ProjectResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<GetProjectByIdQueryHandler> _localizer;

        public GetProjectByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<GetProjectByIdQueryHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<ProjectResponse>> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            var project = await _unitOfWork.Repository<Project>().Entities
                .Where(p => p.Id == request.Id)
                .Include(p => p.Site)
                .Include(p => p.Status)
                .Include(p => p.Category)
                .Include(p => p.Priority)
                .Include(p => p.Client)
                .FirstOrDefaultAsync();
            if (project != null)
            {
                var maapedProject = _mapper.Map<ProjectResponse>(project);
                return await Result<ProjectResponse>.SuccessAsync(maapedProject);
            }
            return await Result<ProjectResponse>.FailAsync(_localizer[$"Project with Id: {request.Id} Not Found"]);
        }
    }
}
