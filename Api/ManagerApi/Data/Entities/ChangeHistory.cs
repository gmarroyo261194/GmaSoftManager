using System.ComponentModel.DataAnnotations;

namespace ManagerApi.Data.Entities;

public class ChangeHistory
{
    [Key]
    public Guid Id { get; set; }

    [Required, StringLength(50)]
    public string EntityType { get; set; }

    [Required]
    public Guid EntityId { get; set; }

    [Required]
    public Guid ChangedById { get; set; }

    [Required, StringLength(50)]
    public string ChangeType { get; set; }

    [Required, StringLength(2000)]
    public string ChangeDescription { get; set; }
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

    public User ChangedBy { get; set; }
}
