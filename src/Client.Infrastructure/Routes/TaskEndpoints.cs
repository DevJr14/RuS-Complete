namespace RuS.Client.Infrastructure.Routes
{
    public static class TaskEndpoints
    {
        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string GetById(int id)
        {
            return $"api/v1/tasks/{id}";
        }

        public static string GetAllForProject(int projectId)
        {
            return $"api/v1/tasks/all/project/{projectId}";
        }

        public static string Export = "api/v1/tasks/export";
        public static string GetAll = "api/v1/tasks/all";
        public static string Delete = "api/v1/tasks";
        public static string Save = "api/v1/tasks";
    }
}

