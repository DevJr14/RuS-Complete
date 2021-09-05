using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuS.Application.Responses.Project.Team
{
    public class TeamMembersResponse
    {
        public List<TeamMemberModel> TeamMembers { get; set; } = new();
    }

    public class TeamMemberModel
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public bool IsSelected { get; set; }
    }
}
