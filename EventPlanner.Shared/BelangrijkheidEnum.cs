using System.Text.Json.Serialization;

namespace EventPlanner.Shared;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum BelangrijkheidEnum
{
    Must,
    Should,
    Could
}
