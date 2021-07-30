using RuS.Application.Interfaces.Common;

namespace RuS.Application.Interfaces.Services
{
    public interface ICurrentUserService : IService
    {
        string UserId { get; }
    }
}