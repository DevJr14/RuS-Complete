namespace RuS.Client.Infrastructure.Routes
{
    public static class TeamEndpoints
    {
        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string GetById(int id)
        {
            return $"api/v1/teams/{id}";
        }

        public static string GetAllForProject(int projectId)
        {
            return $"api/v1/teams/project/{projectId}";
        }

        public static string GetTeamMembers(int teamId)
        {
            return $"api/v1/teams/team-members/{teamId}";
        }

        public static string Export = "api/v1/teams/export";
        public static string GetAll = "api/v1/teams";
        public static string Delete = "api/v1/teams";
        public static string Save = "api/v1/teams";
        public static string SaveMembers = "api/v1/teams/members";
        public static string UpdateTeamMembers = "api/v1/teams/update-members";
    }
}
