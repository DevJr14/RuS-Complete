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

namespace RuS.Application.Features.Projects.Queries.GetAll
{
    public class GetAllProjectsQuery : IRequest<Result<List<ProjectResponse>>>
    {
    }

    internal class GetAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, Result<List<ProjectResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetAllProjectsQueryHandler> _localizer;

        public GetAllProjectsQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<GetAllProjectsQueryHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<List<ProjectResponse>>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        {
            var projects = await _unitOfWork.Repository<Project>().Entities.Include(p => p.Site).ToListAsync();
            if (projects.Count > 0)
            {
                var mappedProjects = _mapper.Map<List<ProjectResponse>>(projects);
                return await Result<List<ProjectResponse>>.SuccessAsync(mappedProjects);
            }
            else
            {
                return await Result<List<ProjectResponse>>.FailAsync(_localizer["No Projects."]);
            }
        }
    }
}
