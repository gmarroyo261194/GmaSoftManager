using System.ComponentModel.DataAnnotations;

namespace ManagerApi.Data.Entities;

public class BugAttachment
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public Guid BugId { get; set; }
    [Required, StringLength(500)]
    public string FilePath { get; set; }
    [Required, StringLength(200)]
    public string FileName { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    public Bug Bug { get; set; }
}