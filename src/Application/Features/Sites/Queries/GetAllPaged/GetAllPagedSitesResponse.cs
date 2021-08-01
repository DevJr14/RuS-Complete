using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Application.Features.Sites.Queries.GetAllPaged
{
    public class GetAllPagedSitesResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CompanyId { get; set; }
        public string Description { get; set; }
    }
}
