using System.ComponentModel.DataAnnotations;

namespace ManagerApi.Data.Entities;

public class ProjectAttachment: AuditEntity
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public Guid ProjectId { get; set; }
    [Required, StringLength(500)]
    public string FilePath { get; set; }
    [Required, StringLength(200)]
    public string FileName { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    public Project Project { get; set; }
}