namespace VebTechTEstApi.Models;

public class Role : DateHistory
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public ICollection<User>? Users { get; set; }
}