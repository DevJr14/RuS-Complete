using System.Text.Json;
using RuS.Application.Interfaces.Serialization.Options;

namespace RuS.Application.Serialization.Options
{
    public class SystemTextJsonOptions : IJsonSerializerOptions
    {
        public JsonSerializerOptions JsonSerializerOptions { get; } = new();
    }
}