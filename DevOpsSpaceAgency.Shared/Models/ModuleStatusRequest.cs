namespace DevOpsSpaceAgency.Shared.Models;

public class ModuleStatusRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
    public string Status { get; set; } = "Unknown";
    public int ResponseTimeMs { get; set; }
    public DateTimeOffset CheckedAt { get; set; }
    public string? ErrorMessage { get; set; }
}
