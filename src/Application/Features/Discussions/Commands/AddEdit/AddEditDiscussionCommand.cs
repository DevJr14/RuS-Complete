using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using RuS.Application.Interfaces.Repositories;
using RuS.Application.Mappings;
using RuS.Domain.Entities.Projects;
using RuS.Shared.Wrapper;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RuS.Application.Features.Discussions.Commands.AddEdit
{
    public class AddEditDiscussionCommand : IRequest<Result<int>>, IMapFrom<Discussion>
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int? TaskId { get; set; }
        public string Comment { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<AddEditDiscussionCommand, Discussion>();
        }
    }

    internal class AddEditDiscussionCommandHandler : IRequestHandler<AddEditDiscussionCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditDiscussionCommandHandler> _localizer;

        public AddEditDiscussionCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditDiscussionCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditDiscussionCommand command, CancellationToken cancellationToken)
        {
            if(command.Id == 0)
            {
                var discussion = _mapper.Map<Discussion>(command);
                await _unitOfWork.Repository<Discussion>().AddAsync(discussion);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(discussion.Id, _localizer["Comment saved."]);
            }
            else
            {
                var discussion = await _unitOfWork.Repository<Discussion>().GetByIdAsync(command.Id);
                if(discussion != null)
                {
                    discussion.Comment = command.Comment ?? discussion.Comment;
                    await _unitOfWork.Repository<Discussion>().UpdateAsync(discussion);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(discussion.Id, _localizer["Comment updated."]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Comment Not Found."]);
                }
            }
        }
    }
}
