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
using System.Threading;
using System.Threading.Tasks;

namespace RuS.Application.Features.Discussions.Queries.GetAll
{
    public class GetAllDiscussionsForProjectQuery : IRequest<Result<List<DiscussionResponse>>>
    {
        public int ProjectId { get; set; }
    }

    internal class GetAllDiscussionsQueryHandler : IRequestHandler<GetAllDiscussionsForProjectQuery, Result<List<DiscussionResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetAllDiscussionsQueryHandler> _localizer;

        public GetAllDiscussionsQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<GetAllDiscussionsQueryHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<List<DiscussionResponse>>> Handle(GetAllDiscussionsForProjectQuery request, CancellationToken cancellationToken)
        {
            var discussions = await _unitOfWork.Repository<Discussion>().Entities
                .Where(d => d.ProjectId == request.ProjectId)
                //.Include(d => d.Project)
                //.Include(d => d.Task)
                .OrderByDescending(d => d.CreatedOn)
                .ToListAsync();
            if(discussions.Count > 0)
            {
                var mappedDiscussions = _mapper.Map<List<DiscussionResponse>>(discussions);
                return await Result<List<DiscussionResponse>>.SuccessAsync(mappedDiscussions);
            }
            return await Result<List<DiscussionResponse>>.FailAsync(_localizer["No Discussions Found."]);
        }
    }
}
