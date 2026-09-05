using System.Text.Json;
using TrueLogs.Contract.Web.Logs;
using TrueLogs.Contract.Web.Logs.Dto;

namespace TrueLogs.Web.Client;

public class LogProviderClient(HttpClient _httpClient) : ILogProviderClient
{
    public async Task<LogsWebContract> Get(int skip, int take, string query, CancellationToken token)
    {
        var endpoint = LogProviderWebEndpoints.LogProviderGetAllEndpoint;
        endpoint = Set(endpoint, nameof(skip), skip.ToString());
        endpoint = Set(endpoint, nameof(take), take.ToString());
        endpoint = Set(endpoint, nameof(query), query.ToString());

        using var responseMessage = await _httpClient.GetAsync(endpoint, token);
        var responseJson = await responseMessage.Content.ReadAsStringAsync(token);
        var response = JsonSerializer.Deserialize<LogsWebContract>(responseJson, Default);

        return response;
    }

    public static JsonSerializerOptions Default = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public static string Set(string endpoint, string key, string value)
        => endpoint.Replace("$" + key, value);
}