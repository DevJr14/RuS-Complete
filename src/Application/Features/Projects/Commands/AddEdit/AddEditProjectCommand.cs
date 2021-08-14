using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using RuS.Application.Features.Sites.Commands.AddEdit;
using RuS.Application.Interfaces.Repositories;
using RuS.Domain.Entities.Core;
using RuS.Domain.Entities.Projects;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RuS.Application.Features.Projects.Commands.AddEdit
{
    public class AddEditProjectCommand : IRequest<Result<int>>
    {
        #region Properties
        public int Id { get; set; }
        public string Name { get; set; }
        public int SiteId { get; set; }
        public int StatusId { get; set; }
        public int CategoryId { get; set; }
        public int PriorityId { get; set; }
        public int ClientId { get; set; }
        public string Description { get; set; }
        public string ScopeOfWork { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public DateTime? ActualStart { get; set; }
        public DateTime? ActualEnd { get; set; }
        #endregion
    }

    internal class AddEditProjectCommandHandler : IRequestHandler<AddEditProjectCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditProjectCommandHandler> _localizer;

        public AddEditProjectCommandHandler(IUnitOfWork<int> unitOfWork, IProjectRepository projectRepository, IMapper mapper, IStringLocalizer<AddEditProjectCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _projectRepository = projectRepository;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditProjectCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var notUnique = await _projectRepository.IsUniqueEntry(command);
                if (notUnique)
                {
                    return await Result<int>.FailAsync(_localizer["Project already exists."]);
                }
                else
                {
                    var project = _mapper.Map<Project>(command);
                    project.Code = _projectRepository.GenerateProjectCode();
                    await _unitOfWork.Repository<Project>().AddAsync(project);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(project.Id, _localizer["Project Saved"]);
                }
            }
            else
            {
                var notUnique = await _projectRepository.IsUniqueEntry(command);
                if (notUnique)
                {
                    return await Result<int>.FailAsync(_localizer["Project already exists."]);
                }
                else
                {
                    var project = await _unitOfWork.Repository<Project>().GetByIdAsync(command.Id);
                    if (project != null)
                    {
                        project.Name = command.Name ?? project.Name;
                        project.Description = command.Description ?? project.Description;
                        project.ScopeOfWork = command.ScopeOfWork ?? project.ScopeOfWork;
                        project.SiteId = command.SiteId;
                        project.StatusId = command.StatusId;
                        project.CategoryId = command.CategoryId;
                        project.PriorityId = command.PriorityId;
                        project.Start = command.Start;
                        project.End = command.End;
                        project.ActualStart = command.ActualStart;
                        project.ActualEnd = command.ActualEnd;
                        await _unitOfWork.Repository<Project>().UpdateAsync(project);
                        await _unitOfWork.Commit(cancellationToken);
                        return await Result<int>.SuccessAsync(project.Id, _localizer["Project Updated"]);
                    }
                    else
                    {
                        return await Result<int>.FailAsync(_localizer["Project Not Found!"]);
                    }
                }
            }
        }
    }
}
