namespace DevOpsSpaceAgency.Shared.Models;

public class AstronautStatusRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SecondaryName { get; set; } = string.Empty;
    public string DisplayName => string.IsNullOrWhiteSpace(SecondaryName) ? Name : SecondaryName;
    public string Craft { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public bool IsCurrentlyOnIss { get; set; }
}
