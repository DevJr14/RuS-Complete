using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Client.Infrastructure.Routes
{
    public static class StatusEndpoints
    {
        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string GetById(int id)
        {
            return $"api/v1/statuses/{id}";
        }

        public static string Export = "api/v1/statuses/export";

        public static string GetAll = "api/v1/statuses";
        public static string Delete = "api/v1/statuses";
        public static string Save = "api/v1/statuses";
    }
}
