using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Application.Features.Employees.Queries.GetById
{
    public class GetEmployeeResponse
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }

        public string CompanyName { get; set; }

        public string EmployeeNo { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }

        public string ImageUrl { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string CellphoneNo { get; set; }

        public string Email { get; set; }

        public string Street { get; set; }

        public string Suburb { get; set; }

        public string City { get; set; }

        public string Postcode { get; set; }
    }
}
