using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Application.Requests.Core
{
    public class GetAllPagedCompaniesRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}
