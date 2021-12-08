namespace RuS.Client.Infrastructure.Routes
{
    public static class DiscussionEndpoints
    {
        public static string GetById(int id)
        {
            return $"api/v1/discussions/{id}";
        }

        public static string GetAllForTask(int taskId)
        {
            return $"api/v1/discussions/all-for-task/{taskId}";
        }

        public static string GetAllForProject(int projectId)
        {
            return $"api/v1/discussions/all-for-project/{projectId}";
        }

        public static string Delete = "api/v1/discussions";
        public static string Save = "api/v1/discussions";
    }
}
