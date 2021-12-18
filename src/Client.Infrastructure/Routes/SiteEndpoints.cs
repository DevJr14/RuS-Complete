using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Client.Infrastructure.Routes
{
    public class SiteEndpoints
    {
        public static string GetAllPaged(int companyId, int pageNumber, int pageSize, string searchString, string[] orderBy)
        {
            var url = $"api/v1/sites/all-paged?companyId={companyId}&pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
            if (orderBy?.Any() == true)
            {
                foreach (var orderByPart in orderBy)
                {
                    url += $"{orderByPart},";
                }
                url = url[..^1]; // loose training ,
            }
            return url;
        }

        public static string ExportFiltered(int comapnyId, string searchString)
        {
            return $"{ExportLink}?companyid={comapnyId}&searchString={searchString}";
        }

        public static string Export(int companyId)
        {
            return $"{ExportLink}?companyid={companyId}";
        }

        public static string GetById(int id)
        {
            return $"api/v1/sites/{id}";
        }

        public static string Save = "api/v1/sites";
        public static string GetAll = "api/v1/sites/all";
        public static string Delete = "api/v1/sites";
        public static string ExportLink = "api/v1/sites/export";
    }
}
