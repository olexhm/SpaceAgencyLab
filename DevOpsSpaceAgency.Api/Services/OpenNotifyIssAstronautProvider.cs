using System.Text.Json;
using DevOpsSpaceAgency.BLL.Services.Interfaces;
using DevOpsSpaceAgency.Shared.Models;

namespace DevOpsSpaceAgency.Api.Services;

public class OpenNotifyIssAstronautProvider : IIssAstronautProvider
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
    };

    private readonly HttpClient _httpClient;

    public OpenNotifyIssAstronautProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<PersonRequest>> GetCurrentIssAstronautsAsync(CancellationToken cancellationToken = default)
    {
        using HttpResponseMessage response = await _httpClient.GetAsync("astros", cancellationToken);
        response.EnsureSuccessStatusCode();

        await using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        TeamRequest? teamRequest = await JsonSerializer.DeserializeAsync<TeamRequest>(responseStream, JsonOptions, cancellationToken);

        return teamRequest?.People
            .Where(person => string.Equals(person.Craft, "ISS", StringComparison.OrdinalIgnoreCase))
            .Select(person => new PersonRequest
            {
                Name = person.Name.Trim(),
                Craft = person.Craft.Trim(),
            })
            .ToList() ?? [];
    }
}
