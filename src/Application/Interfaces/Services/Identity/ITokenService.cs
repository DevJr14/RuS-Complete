using RuS.Application.Interfaces.Common;
using RuS.Application.Requests.Identity;
using RuS.Application.Responses.Identity;
using RuS.Shared.Wrapper;
using System.Threading.Tasks;

namespace RuS.Application.Interfaces.Services.Identity
{
    public interface ITokenService : IService
    {
        Task<Result<TokenResponse>> LoginAsync(TokenRequest model);

        Task<Result<TokenResponse>> GetRefreshTokenAsync(RefreshTokenRequest model);
    }
}