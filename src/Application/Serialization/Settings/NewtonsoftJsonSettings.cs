
using RuS.Application.Interfaces.Serialization.Settings;
using Newtonsoft.Json;

namespace RuS.Application.Serialization.Settings
{
    public class NewtonsoftJsonSettings : IJsonSerializerSettings
    {
        public JsonSerializerSettings JsonSerializerSettings { get; } = new();
    }
}