using System.Net.Http.Json;
using DevOpsSpaceAgency.Shared.Models;

namespace DevOpsSpaceAgency.Web.Services;

public class SpaceApiClient
{
    private readonly HttpClient _httpClient;

    public SpaceApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<AstronautStatusRequest>> GetAstronautsAsync(CancellationToken cancellationToken = default)
        => await _httpClient.GetFromJsonAsync<List<AstronautStatusRequest>>("api/Persons/iss", cancellationToken) ?? [];

    public async Task<IssPositionRequest> GetIssPositionAsync(CancellationToken cancellationToken = default)
        => await _httpClient.GetFromJsonAsync<IssPositionRequest>("api/Space/iss-position", cancellationToken) ?? new IssPositionRequest();

    public async Task<ImageRequest> GetTodayImageAsync(CancellationToken cancellationToken = default)
        => await _httpClient.GetFromJsonAsync<ImageRequest>("api/Space/apod", cancellationToken) ?? new ImageRequest();

    public async Task<IReadOnlyList<ModuleStatusRequest>> GetModuleStatusesAsync(CancellationToken cancellationToken = default)
        => await _httpClient.GetFromJsonAsync<List<ModuleStatusRequest>>("api/Space/modules/status", cancellationToken) ?? [];

    public Task<HttpResponseMessage> CreateAstronautAsync(PersonRequest request, CancellationToken cancellationToken = default)
        => _httpClient.PostAsJsonAsync("api/Persons", request, cancellationToken);

    public Task<HttpResponseMessage> UpdateAstronautAsync(PersonRequest request, CancellationToken cancellationToken = default)
        => _httpClient.PutAsJsonAsync($"api/Persons/{request.Id}", request, cancellationToken);

    public Task<HttpResponseMessage> DeleteAstronautAsync(int astronautId, CancellationToken cancellationToken = default)
        => _httpClient.DeleteAsync($"api/Persons/{astronautId}", cancellationToken);
}
