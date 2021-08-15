using AutoMapper;
using RuS.Application.Features.Clients.Commands.AddEdit;
using RuS.Application.Features.Clients.Queries;
using RuS.Domain.Entities.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Application.Mappings
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<AddEditClientCommand, Client>();
            CreateMap<Client, ClientResponse>();
        }
    }
}
