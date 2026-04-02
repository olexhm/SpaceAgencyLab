using System.Text.Json;
using DevOpsSpaceAgency.BLL.Services.Interfaces;
using DevOpsSpaceAgency.Shared.Models;

namespace DevOpsSpaceAgency.Api.Services;

public class NasaApodProvider : IApodProvider
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    private readonly HttpClient _httpClient;

    public NasaApodProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ImageRequest> GetTodayImageAsync(CancellationToken cancellationToken = default)
    {
        using HttpResponseMessage response = await _httpClient.GetAsync("planetary/apod?api_key=DEMO_KEY", cancellationToken);
        response.EnsureSuccessStatusCode();

        await using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        return await JsonSerializer.DeserializeAsync<ImageRequest>(responseStream, JsonOptions, cancellationToken)
            ?? new ImageRequest();
    }
}
