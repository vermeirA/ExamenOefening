using System.Text.Json.Serialization;

namespace EventPlanner.Shared;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum StatusEnum
{
    Todo,
    Doing,
    Done,
    Cancelled
}
