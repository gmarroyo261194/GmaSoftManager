using ManagerApi.Data.Entities;

namespace ManagerApi.DTOs.Bugs;

public class BugDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Severity { get; set; }
    public string Status { get; set; }
    
    public Guid ProjectId { get; set; }
    public string ProjectName { get; set; }
    
    public Guid CreatedById { get; set; }
    public string CreatedByName { get; set; }
    
    public Guid? AssignedToId { get; set; }
    public string AssignedToName { get; set; }
    
    public Guid? FixedInTaskId { get; set; }
    public string FixedInTaskTitle { get; set; }
    
    public DateTime CreatedAt { get; set; }
}
