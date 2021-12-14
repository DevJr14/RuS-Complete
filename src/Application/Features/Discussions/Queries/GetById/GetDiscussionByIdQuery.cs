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

namespace RuS.Application.Features.Discussions.Queries.GetById
{
    public class GetDiscussionByIdQuery : IRequest<Result<DiscussionResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetDiscussionByIdQueryHandler : IRequestHandler<GetDiscussionByIdQuery, Result<DiscussionResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetDiscussionByIdQueryHandler> _localizer;

        public GetDiscussionByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<GetDiscussionByIdQueryHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<DiscussionResponse>> Handle(GetDiscussionByIdQuery request, CancellationToken cancellationToken)
        {
            var discussion = await _unitOfWork.Repository<Discussion>().Entities
                .Where(d => d.Id == request.Id)
                .Include(d => d.Project)
                .Include(d => d.Task)
                .FirstOrDefaultAsync();

            if(discussion != null)
            {
                var mappedDiscussion = _mapper.Map<DiscussionResponse>(discussion);
                return await Result<DiscussionResponse>.SuccessAsync(mappedDiscussion);
            }
            return await Result<DiscussionResponse>.FailAsync(_localizer["Discussion Not Found."]);
        }
    }
}
