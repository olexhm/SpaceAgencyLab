namespace DevOpsSpaceAgency.Shared.Models;

public class PersonRequest
{
    public int Id { get; set; }
    public string Craft { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string SecondaryName { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    public string DisplayName => string.IsNullOrWhiteSpace(SecondaryName) ? Name : SecondaryName;
}
