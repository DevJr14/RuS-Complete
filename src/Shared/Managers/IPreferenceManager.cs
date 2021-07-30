using RuS.Shared.Settings;
using System.Threading.Tasks;
using RuS.Shared.Wrapper;

namespace RuS.Shared.Managers
{
    public interface IPreferenceManager
    {
        Task SetPreference(IPreference preference);

        Task<IPreference> GetPreference();

        Task<IResult> ChangeLanguageAsync(string languageCode);
    }
}