using System.ComponentModel.DataAnnotations;

namespace ManagerApi.Data.Entities;

public class Project: AuditEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required, StringLength(200)]
    public string Name { get; set; }

    [StringLength(1000)]
    public string Description { get; set; }

    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EstimatedEndDate { get; set; }
    public DateTime? ActualEndDate { get; set; }
    [Required, StringLength(30)]
    public string Status { get; set; } // Creado, En progreso, etc.

    // Navigation
    public ICollection<ProjectMember> Members { get; set; }
    public ICollection<ProjectTask> Tasks { get; set; }
    public ICollection<Milestone> Milestones { get; set; }
    public ICollection<Bug> Bugs { get; set; }
    public ICollection<ProjectAttachment> Attachments { get; set; }
}