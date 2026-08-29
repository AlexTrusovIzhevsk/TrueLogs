using System.Text;
using System.Text.Json;
using TrueLogs.Contract.Clients;
using TrueLogs.Contract.Emitter;
using TrueLogs.Contract.Emitter.Dto;

namespace TrueLogs.Emitter.Client;

public class LogEmitterClient(HttpClient _httpClient) : ILogEmitterClient
{
    public async Task Emit(IEnumerable<LogEmitterContract> logs, CancellationToken token)
    {
        var json = JsonSerializer.Serialize(logs, Default);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        using var responseMessage = await _httpClient.PostAsync(EmitterEndpoints.LogEmitterRoute, content, token);
    }

    public static JsonSerializerOptions Default = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };
}