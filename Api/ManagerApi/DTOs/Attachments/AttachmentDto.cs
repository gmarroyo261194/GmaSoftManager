using System.ComponentModel.DataAnnotations;

namespace ManagerApi.DTOs.Attachments;

public class AttachmentDto
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public DateTime UploadedAt { get; set; }
    public Guid EntityId { get; set; }
    public string EntityType { get; set; } // Task, Bug, Project
}

public class CreateAttachmentDto
{
    [Required]
    public string FileName { get; set; }
    [Required]
    public string FilePath { get; set; } // In a real app, this would be an IFormFile
}
