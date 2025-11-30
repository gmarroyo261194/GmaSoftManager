using System.ComponentModel.DataAnnotations;

namespace ManagerApi.DTOs.Milestones;

public class MilestoneDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime TargetDate { get; set; }
    public string Status { get; set; }
    public Guid ProjectId { get; set; }
}
