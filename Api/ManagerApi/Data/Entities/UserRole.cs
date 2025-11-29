using System.ComponentModel.DataAnnotations;

namespace ManagerApi.Data.Entities;

public class UserRole
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid UserId { get; set; }
    [Required, StringLength(50)]
    public string Role { get; set; } // Ej: Admin, Leader, Developer, QA, Cliente

    public User User { get; set; }
}