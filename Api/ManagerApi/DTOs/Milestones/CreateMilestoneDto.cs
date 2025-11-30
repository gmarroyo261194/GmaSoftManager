using System.ComponentModel.DataAnnotations;

namespace ManagerApi.DTOs.Milestones;

public class CreateMilestoneDto
{
    [Required, StringLength(100)]
    public string Name { get; set; }

    [Required]
    public DateTime TargetDate { get; set; }

    [Required]
    public Guid ProjectId { get; set; }
}
