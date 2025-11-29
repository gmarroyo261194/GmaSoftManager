using System.ComponentModel.DataAnnotations;

namespace ManagerApi.Data.Entities;

public class Notification
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public Guid UserId { get; set; }
    [Required, StringLength(50)]
    public string NotificationType { get; set; }
    [Required, StringLength(50)]
    public string EntityType { get; set; }
    [Required]
    public Guid EntityId { get; set; }
    [Required, StringLength(500)]
    public string Message { get; set; }
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; }
}
