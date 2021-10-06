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

namespace RuS.Application.Features.Clients.Commands.AddEdit
{
    public class AddEditClientCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public string TelephoneNo { get; set; }
        public string CellphoneNo { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string Suburb { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
    }

    internal class AddEditClientCommandHandler : IRequestHandler<AddEditClientCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IClientRepository _clientRepository;
        private readonly IStringLocalizer<AddEditClientCommandHandler> _localizer;

        public AddEditClientCommandHandler(IUnitOfWork<int> unitOfWork, IClientRepository clientRepository, IMapper mapper, IStringLocalizer<AddEditClientCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _clientRepository = clientRepository;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditClientCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var notUnique = await _clientRepository.IsUniqueEntry(command);
                if (notUnique)
                {
                    return await Result<int>.FailAsync(_localizer["Client already exists."]);
                }
                else
                {
                    var client = _mapper.Map<Client>(command);
                    await _unitOfWork.Repository<Client>().AddAsync(client);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(client.Id, _localizer["Client Saved"]);
                }
            }
            else
            {
                var notUnique = await _clientRepository.IsUniqueEntry(command);
                if (notUnique)
                {
                    return await Result<int>.FailAsync(_localizer["Client already exists."]);
                }
                var client = await _unitOfWork.Repository<Client>().GetByIdAsync(command.Id);
                if (client != null)
                {
                    client.Name = command.Name;

                    client.ContactPerson = command.ContactPerson ?? client.ContactPerson;
                    client.CellphoneNo = command.CellphoneNo ?? client.CellphoneNo;
                    client.TelephoneNo = command.TelephoneNo ?? client.TelephoneNo;
                    client.Street = command.Street ?? client.Street;
                    client.Suburb = command.Suburb ?? client.Suburb;
                    client.City = command.City ?? client.City;
                    client.Postcode = command.Postcode ?? client.Postcode;
                    client.Email = command.Email ?? client.Email;

                    await _unitOfWork.Repository<Client>().UpdateAsync(client);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(client.Id, _localizer["Client Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Client Not Found!"]);
                }
            }
        }
    }
}
