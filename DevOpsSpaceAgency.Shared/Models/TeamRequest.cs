namespace DevOpsSpaceAgency.Shared.Models;

public class TeamRequest
{
    public List<PersonRequest> People { get; set; }
    public int Number { get;set; }
    public string Message { get;set; }
}