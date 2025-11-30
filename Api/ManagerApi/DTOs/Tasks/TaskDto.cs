using ManagerApi.Data.Entities;

namespace ManagerApi.DTOs.Tasks;

public class TaskDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public string Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid ProjectId { get; set; }
    public string ProjectName { get; set; }
    public Guid? AssignedToId { get; set; }
    public string AssignedToName { get; set; }
    public DateTime CreatedAt { get; set; }
}
