using System.Collections.Generic;
using System.Threading.Tasks;
using RuS.Application.Requests.Identity;
using RuS.Application.Responses.Identity;
using RuS.Shared.Wrapper;

namespace RuS.Client.Infrastructure.Managers.Identity.RoleClaims
{
    public interface IRoleClaimManager : IManager
    {
        Task<IResult<List<RoleClaimResponse>>> GetRoleClaimsAsync();

        Task<IResult<List<RoleClaimResponse>>> GetRoleClaimsByRoleIdAsync(string roleId);

        Task<IResult<string>> SaveAsync(RoleClaimRequest role);

        Task<IResult<string>> DeleteAsync(string id);
    }
}