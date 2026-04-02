using System.Diagnostics;
using DevOpsSpaceAgency.BLL.Services.Interfaces;
using DevOpsSpaceAgency.Shared.Models;

namespace DevOpsSpaceAgency.Api.Services;

public class HttpStatModuleStatusProvider : IModuleStatusProvider
{
    private static readonly ModuleDefinition[] Modules =
    [
        new("Mission Control API", "Nominal response path.", "200"),
        new("Telemetry Uplink", "Simulated degraded or unavailable dependency.", "503"),
        new("Deep Space Relay", "Slow response path for timeout monitoring.", "200?sleep=5000"),
    ];

    private readonly HttpClient _httpClient;

    public HttpStatModuleStatusProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.Timeout = TimeSpan.FromSeconds(3);
    }

    public async Task<IReadOnlyList<ModuleStatusRequest>> CheckModulesAsync(CancellationToken cancellationToken = default)
    {
        List<ModuleStatusRequest> results = [];

        foreach (ModuleDefinition module in Modules)
        {
            results.Add(await CheckModuleAsync(module, cancellationToken));
        }

        return results;
    }

    private async Task<ModuleStatusRequest> CheckModuleAsync(ModuleDefinition module, CancellationToken cancellationToken)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        try
        {
            using HttpResponseMessage response = await _httpClient.GetAsync(module.Endpoint, cancellationToken);
            stopwatch.Stop();

            return new ModuleStatusRequest
            {
                Name = module.Name,
                Description = module.Description,
                Endpoint = new Uri(_httpClient.BaseAddress!, module.Endpoint).ToString(),
                Status = response.IsSuccessStatusCode ? "Online" : "Offline",
                ResponseTimeMs = (int)stopwatch.ElapsedMilliseconds,
                CheckedAt = DateTimeOffset.UtcNow,
                ErrorMessage = response.IsSuccessStatusCode ? null : $"HTTP {(int)response.StatusCode}",
            };
        }
        catch (TaskCanceledException)
        {
            stopwatch.Stop();
            return new ModuleStatusRequest
            {
                Name = module.Name,
                Description = module.Description,
                Endpoint = new Uri(_httpClient.BaseAddress!, module.Endpoint).ToString(),
                Status = "Degraded",
                ResponseTimeMs = (int)stopwatch.ElapsedMilliseconds,
                CheckedAt = DateTimeOffset.UtcNow,
                ErrorMessage = "Request timed out.",
            };
        }
        catch (HttpRequestException exception)
        {
            stopwatch.Stop();
            return new ModuleStatusRequest
            {
                Name = module.Name,
                Description = module.Description,
                Endpoint = new Uri(_httpClient.BaseAddress!, module.Endpoint).ToString(),
                Status = "Offline",
                ResponseTimeMs = (int)stopwatch.ElapsedMilliseconds,
                CheckedAt = DateTimeOffset.UtcNow,
                ErrorMessage = exception.Message,
            };
        }
    }

    private sealed record ModuleDefinition(string Name, string Description, string Endpoint);
}
