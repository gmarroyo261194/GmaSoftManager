using System.ComponentModel.DataAnnotations;

namespace ManagerApi.Data.Entities;

public class BugComment
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public Guid BugId { get; set; }
    [Required]
    public Guid UserId { get; set; }
    [Required, StringLength(2000)]
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Bug Bug { get; set; }
    public User User { get; set; }
}