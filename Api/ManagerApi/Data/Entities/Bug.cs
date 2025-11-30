using System.ComponentModel.DataAnnotations;

namespace ManagerApi.Data.Entities;

public class Bug: AuditEntity
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public Guid ProjectId { get; set; }
    [Required, StringLength(200)]
    public string Title { get; set; }
    [StringLength(1000)]
    public string Description { get; set; }
    [Required, StringLength(30)]
    public string Status { get; set; }
    [Required, StringLength(20)]
    public string Severity { get; set; } // Baja, Media, Alta, Crítica

    public Guid? AssignedToId { get; set; }
    public Guid? FixedInTaskId { get; set; }

    public Project Project { get; set; }
    public User CreatedBy { get; set; }
    public User AssignedTo { get; set; }
    public ProjectTask FixedInTask { get; set; }

    public ICollection<BugComment> Comments { get; set; }
    public ICollection<BugAttachment> Attachments { get; set; }
    public ICollection<BugTag> Tags { get; set; }
}