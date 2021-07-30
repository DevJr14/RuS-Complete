using RuS.Shared.Wrapper;
using System.Threading.Tasks;
using RuS.Application.Features.Dashboards.Queries.GetData;

namespace RuS.Client.Infrastructure.Managers.Dashboard
{
    public interface IDashboardManager : IManager
    {
        Task<IResult<DashboardDataResponse>> GetDataAsync();
    }
}