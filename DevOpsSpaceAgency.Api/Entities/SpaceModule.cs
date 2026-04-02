namespace DevOpsSpaceAgency.Api.Models;

public class SpaceModule
{
    public int Id { get; set; }
    public string Name { get; set; }       // ex: "Propulsion"
    public string Description { get; set; }
    public ModuleStatus Status { get; set; } // Online, Offline, Degraded
    public DateTime? LastChecked { get; set; }
}

public enum ModuleStatus { Unknown, Online, Degraded, Offline }