using System.Collections.Generic;
using System.Threading.Tasks;
using RuS.Application.Interfaces.Common;
using RuS.Application.Requests.Identity;
using RuS.Application.Responses.Identity;
using RuS.Shared.Wrapper;

namespace RuS.Application.Interfaces.Services.Identity
{
    public interface IRoleClaimService : IService
    {
        Task<Result<List<RoleClaimResponse>>> GetAllAsync();

        Task<int> GetCountAsync();

        Task<Result<RoleClaimResponse>> GetByIdAsync(int id);

        Task<Result<List<RoleClaimResponse>>> GetAllByRoleIdAsync(string roleId);

        Task<Result<string>> SaveAsync(RoleClaimRequest request);

        Task<Result<string>> DeleteAsync(int id);
    }
}