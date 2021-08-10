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

namespace RuS.Application.Features.Statuses.Commands
{
    public class AddEditStatusCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    internal class AddEditStatusCommandHandler : IRequestHandler<AddEditStatusCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStatusRepository _statusRepository;
        private readonly IStringLocalizer<AddEditStatusCommandHandler> _localizer;

        public AddEditStatusCommandHandler(IUnitOfWork<int> unitOfWork, IStatusRepository statusRepository, IStringLocalizer<AddEditStatusCommandHandler> localizer, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _statusRepository = statusRepository;
            _localizer = localizer;
            _mapper = mapper;
        }


        public async Task<Result<int>> Handle(AddEditStatusCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var notUnique = await _statusRepository.IsUniqueEntry(command.Name);
                if (notUnique)
                {
                    return await Result<int>.FailAsync(_localizer["Status already exists."]);
                }
                else
                {
                    var status = _mapper.Map<Status>(command);
                    await _unitOfWork.Repository<Status>().AddAsync(status);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(status.Id, _localizer["Status Saved"]);
                }
            }
            else
            {
                var notUnique = await _statusRepository.IsUniqueEntry(command.Name, command.Id);
                if (notUnique)
                {
                    return await Result<int>.FailAsync(_localizer["Status already exists."]);
                }
                else
                {
                    var status = await _unitOfWork.Repository<Status>().GetByIdAsync(command.Id);
                    if (status != null)
                    {
                        status.Name = command.Name ?? status.Name;
                        status.Description = command.Description ?? status.Description;
                        await _unitOfWork.Repository<Status>().UpdateAsync(status);
                        await _unitOfWork.Commit(cancellationToken);
                        return await Result<int>.SuccessAsync(status.Id, _localizer["Status Updated"]);
                    }
                    else
                    {
                        return await Result<int>.FailAsync(_localizer["Status Not Found!"]);
                    }
                }
            }
        }
    }
}
