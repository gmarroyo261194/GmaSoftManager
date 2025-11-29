using System.ComponentModel.DataAnnotations;

namespace ManagerApi.Data.Entities;

public class GeneralAttachment
{
    [Key]
    public Guid Id { get; set; }
    [Required, StringLength(50)]
    public string EntityType { get; set; } // Project, Task, Bug, Milestone, etc.
    [Required]
    public Guid EntityId { get; set; }
    [Required, StringLength(500)]
    public string FilePath { get; set; }
    [Required, StringLength(200)]
    public string FileName { get; set; }
    [Required]
    public Guid UploadedById { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    public User UploadedBy { get; set; }
}