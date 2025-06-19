namespace WebApi.Models;

public class Project
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    [UseSorting]
    public List<WorkTask> Tasks { get; set; } = new List<WorkTask>();
}
