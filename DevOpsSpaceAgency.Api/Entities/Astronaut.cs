namespace DevOpsSpaceAgency.Api.Models;

public class Astronaut
{
    public Astronaut(int id, string name, string country)
    {
        Id = id;
        Name = name;
        Country = country;
    }

    public Astronaut()
    {
    }

    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Country { get; set; }
}