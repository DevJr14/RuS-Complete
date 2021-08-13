using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using RuS.Application.Interfaces.Repositories;
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
            throw new NotImplementedException();
        }
    }
}
