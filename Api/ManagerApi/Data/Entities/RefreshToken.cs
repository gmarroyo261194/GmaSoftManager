using System.ComponentModel.DataAnnotations;

namespace ManagerApi.Data.Entities;

public class RefreshToken
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public Guid UserId { get; set; }
    [Required, StringLength(255)]
    public string Token { get; set; }
    public DateTime ExpiryDateUtc { get; set; }
    public bool IsRevoked { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; }
}