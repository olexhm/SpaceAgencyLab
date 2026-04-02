using System.Text.Json;
using DevOpsSpaceAgency.BLL.Services.Interfaces;
using DevOpsSpaceAgency.Shared.Models;

namespace DevOpsSpaceAgency.Api.Services;

public class OpenNotifyIssPositionProvider : IIssPositionProvider
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
    };

    private readonly HttpClient _httpClient;

    public OpenNotifyIssPositionProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IssPositionRequest> GetCurrentPositionAsync(CancellationToken cancellationToken = default)
    {
        using HttpResponseMessage response = await _httpClient.GetAsync("iss-now.json", cancellationToken);
        response.EnsureSuccessStatusCode();

        await using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        OpenNotifyPositionEnvelope? envelope = await JsonSerializer.DeserializeAsync<OpenNotifyPositionEnvelope>(responseStream, JsonOptions, cancellationToken);

        return new IssPositionRequest
        {
            Latitude = envelope?.IssPosition?.Latitude ?? string.Empty,
            Longitude = envelope?.IssPosition?.Longitude ?? string.Empty,
            Message = envelope?.Message ?? string.Empty,
            Timestamp = envelope?.Timestamp ?? 0,
        };
    }

    private sealed class OpenNotifyPositionEnvelope
    {
        public OpenNotifyPosition? IssPosition { get; set; }
        public string Message { get; set; } = string.Empty;
        public int Timestamp { get; set; }
    }

    private sealed class OpenNotifyPosition
    {
        public string Latitude { get; set; } = string.Empty;
        public string Longitude { get; set; } = string.Empty;
    }
}
