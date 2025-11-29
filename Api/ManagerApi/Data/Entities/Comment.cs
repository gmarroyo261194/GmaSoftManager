using System.ComponentModel.DataAnnotations;

namespace ManagerApi.Data.Entities;

public class Comment : AuditEntity
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public Guid TaskId { get; set; }
    [Required]
    public Guid UserId { get; set; }
    [Required, StringLength(2000)]
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ProjectTask Task { get; set; }
    public User User { get; set; }
}