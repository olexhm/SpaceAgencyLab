namespace DevOpsSpaceAgency.Shared.Models;

public class IssPositionRequest
{
    public string Longitude { get; set; } = string.Empty;
    public string Latitude { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public int Timestamp { get; set; }
}
