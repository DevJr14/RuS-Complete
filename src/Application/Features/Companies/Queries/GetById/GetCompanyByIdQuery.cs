using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RuS.Application.Interfaces.Repositories;
using RuS.Domain.Entities.Core;
using RuS.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RuS.Application.Features.Companies.Queries.GetById
{
    public class GetCompanyByIdQuery : IRequest<Result<GetCompanyResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetCompanyByIdQueryHandler : IRequestHandler<GetCompanyByIdQuery, Result<GetCompanyResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetCompanyByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetCompanyResponse>> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
        {
            var company = await _unitOfWork.Repository<Company>().Entities
                .Where(c => c.Id == request.Id)
                .Include(c => c.Sites)
                .FirstOrDefaultAsync();
            var mappedCompany = _mapper.Map<GetCompanyResponse>(company);
            return await Result<GetCompanyResponse>.SuccessAsync(mappedCompany);
        }
    }
}
