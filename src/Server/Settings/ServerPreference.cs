using System.Linq;
using RuS.Shared.Constants.Localization;
using RuS.Shared.Settings;

namespace RuS.Server.Settings
{
    public record ServerPreference : IPreference
    {
        public string LanguageCode { get; set; } = LocalizationConstants.SupportedLanguages.FirstOrDefault()?.Code ?? "en-US";

        //TODO - add server preferences
    }
}