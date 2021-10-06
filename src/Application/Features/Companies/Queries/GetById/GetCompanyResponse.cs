using RuS.Application.Features.Sites.Queries.GetById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Application.Features.Companies.Queries.GetById
{
    public class GetCompanyResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortTitle { get; set; }
        public string RegistrationNo { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public List<SiteResponse> Sites { get; set; } = new();
    }
}
