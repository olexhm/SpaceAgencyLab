namespace DevOpsSpaceAgency.DAL.Entities;

public class PersonEntity
{
    public int Id { get; set; }
    public string OfficialName { get; set; } = string.Empty;
    public string SecondaryName { get; set; } = string.Empty;
    public string Craft { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}
