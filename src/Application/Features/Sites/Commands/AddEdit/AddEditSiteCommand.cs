using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using RuS.Application.Interfaces.Repositories;
using RuS.Domain.Entities.Core;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RuS.Application.Features.Sites.Commands.AddEdit
{
    public class AddEditSiteCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CompanyId { get; set; }
        public string Description { get; set; }
    }

    public class AddEditSiteCommandHandler : IRequestHandler<AddEditSiteCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ISiteRepository _siteRepository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditSiteCommandHandler> _localizer;
        public AddEditSiteCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditSiteCommandHandler> localizer, ISiteRepository siteRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
            _siteRepository = siteRepository;
        }

        public async Task<Result<int>> Handle(AddEditSiteCommand command, CancellationToken cancellationToken)
        {
            
            if (command.Id == 0)
            {
                var notUnique = await _siteRepository.IsUniqueEntry(command.Name, command.CompanyId);
                if (notUnique)
                {
                    return await Result<int>.FailAsync(_localizer["Site already exists."]);
                }
                else
                {
                    var site = _mapper.Map<Site>(command);
                    await _unitOfWork.Repository<Site>().AddAsync(site);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(site.Id, _localizer["Site Saved"]);
                }
            }
            else
            {
                var notUnique = await _siteRepository.IsUniqueEntry(command.Name, command.CompanyId, command.Id);
                if (notUnique)
                {
                    return await Result<int>.FailAsync(_localizer["Site already exists."]);
                }
                else
                {
                    var site = await _unitOfWork.Repository<Site>().GetByIdAsync(command.Id);
                    if (site != null)
                    {
                        site.Name = command.Name ?? site.Name;
                        site.Description = command.Description ?? site.Description;
                        await _unitOfWork.Repository<Site>().UpdateAsync(site);
                        await _unitOfWork.Commit(cancellationToken);
                        return await Result<int>.SuccessAsync(site.Id, _localizer["Site Updated"]);
                    }
                    else
                    {
                        return await Result<int>.FailAsync("Site Not Found!");
                    }
                }
            }
        }
    }
}
