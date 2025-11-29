using System.ComponentModel.DataAnnotations;

namespace ManagerApi.Data.Entities;

public class ProjectTask : AuditEntity
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public Guid ProjectId { get; set; }
    public Guid? AssignedToId { get; set; }
    [Required, StringLength(200)]
    public string Title { get; set; }
    [StringLength(1000)]
    public string Description { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [Required]
    public DateTime DueDate { get; set; }
    [Required, StringLength(30)]
    public string Status { get; set; } // Pendiente, En progreso, etc.
    [Required, StringLength(15)]
    public string Priority { get; set; } = "Normal";

    public Project Project { get; set; }
    public User AssignedTo { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public ICollection<Attachment> Attachments { get; set; }
    public ICollection<TaskTag> Tags { get; set; }
    public ICollection<Bug> FixedBugs { get; set; }
}