namespace WebApi.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    [UseSorting]
    public List<WorkTask> Tasks { get; set; } = new List<WorkTask>();
}
