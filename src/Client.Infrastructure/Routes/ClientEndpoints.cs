using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Client.Infrastructure.Routes
{
    public static class ClientEndpoints
    {
        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string GetById(int id)
        {
            return $"api/v1/clients/{id}";
        }

        public static string Export = "api/v1/clients/export";

        public static string GetAll = "api/v1/clients";
        public static string Delete = "api/v1/clients";
        public static string Save = "api/v1/clients";
    }
}
