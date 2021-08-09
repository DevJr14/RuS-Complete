using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Application.Features.Companies.Queries.GetAll
{
    public class GetAllCompaniesResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ShortTitle { get; set; }

        public string RegistrationNo { get; set; }

        public DateTime? RegistrationDate { get; set; }
    }
}
