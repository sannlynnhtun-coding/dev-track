using System.Net.Http.Json;
using System.Text.Json;
using DevTrack.Shared;

namespace DevTrack.WebApp.Services;

public abstract class ApiClientBase
{
    public const string ClientName = "DevTrackApi";

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly IHttpClientFactory _httpClientFactory;

    protected ApiClientBase(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    protected async Task<TResponse> GetAsync<TResponse>(string requestUri)
    {
        var client = CreateClient();
        using var response = await client.GetAsync(requestUri);
        return await ReadResponseAsync<TResponse>(response);
    }

    protected async Task<TResponse> PostAsync<TRequest, TResponse>(string requestUri, TRequest request)
    {
        var client = CreateClient();
        using var response = await client.PostAsJsonAsync(requestUri, request, JsonOptions);
        return await ReadResponseAsync<TResponse>(response);
    }

    protected static string WithPagination(string requestUri, PaginationRequest request)
        => $"{requestUri}?PageNumber={request.PageNumber}&PageSize={request.PageSize}";

    private HttpClient CreateClient()
        => _httpClientFactory.CreateClient(ClientName);

    private static async Task<TResponse> ReadResponseAsync<TResponse>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            var message = GetReadableApiError(content) ?? response.ReasonPhrase ?? "API request failed.";
            throw new HttpRequestException(message, null, response.StatusCode);
        }

        if (string.IsNullOrWhiteSpace(content))
        {
            throw new InvalidOperationException("API response body was empty.");
        }

        var result = JsonSerializer.Deserialize<TResponse>(content, JsonOptions);
        return result ?? throw new InvalidOperationException("API response body could not be deserialized.");
    }

    private static string? GetReadableApiError(string rawMessage)
    {
        if (string.IsNullOrWhiteSpace(rawMessage))
        {
            return null;
        }

        try
        {
            using var doc = JsonDocument.Parse(rawMessage);
            var root = doc.RootElement;

            if (root.TryGetProperty("errors", out var errors) && errors.ValueKind == JsonValueKind.Object)
            {
                var firstError = errors.EnumerateObject()
                    .SelectMany(p => p.Value.EnumerateArray().Select(v => v.GetString()))
                    .FirstOrDefault(v => !string.IsNullOrWhiteSpace(v));
                if (!string.IsNullOrWhiteSpace(firstError))
                {
                    return firstError;
                }
            }

            if (root.TryGetProperty("detail", out var detail) && detail.GetString() is { Length: > 0 } detailText)
            {
                return detailText;
            }

            if (root.TryGetProperty("title", out var title) && title.GetString() is { Length: > 0 } titleText)
            {
                return titleText;
            }
        }
        catch (JsonException)
        {
            return rawMessage;
        }

        return rawMessage;
    }
}
