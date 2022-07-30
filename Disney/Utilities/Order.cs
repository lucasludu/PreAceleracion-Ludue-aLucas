using System.Text.Json.Serialization;

namespace Disney.Utilities
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Order
    {
        ASC,
        DESC,
        None
    }
}
