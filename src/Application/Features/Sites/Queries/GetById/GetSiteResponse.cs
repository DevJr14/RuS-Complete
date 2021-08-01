using RuS.Application.Features.Companies.Queries.GetById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Application.Features.Sites.Queries.GetById
{
    public class GetSiteResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CompanyId { get; set; }
        public string Description { get; set; }
        public GetCompanyResponse Company { get; set; }
    }
}
