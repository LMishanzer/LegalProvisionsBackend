using Newtonsoft.Json;

namespace LegalProvisionsLib.Logging.Models;

public class RequestLog : LogItem
{
    [JsonProperty("ip")]
    public required string Ip { get; init; }
    [JsonProperty("requestDuration")]
    public required double RequestDuration { get; init; }
    [JsonProperty("url")]
    public required string Url { get; init; }
    [JsonProperty("body")]
    public required string Body { get; init; }
    [JsonProperty("method")]
    public required string Method { get; init; }
    [JsonProperty("statusCode")]
    public required int StatusCode { get; init; }
}