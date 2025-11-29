using System.ComponentModel.DataAnnotations;

namespace ManagerApi.Data.Entities;

public class ProjectMember: AuditEntity
{
    [Required]
    public Guid ProjectId { get; set; }
    [Required]
    public Guid UserId { get; set; }
    [Required, StringLength(50)]
    public string Role { get; set; }

    public Project Project { get; set; }
    public User User { get; set; }

    // Clave primaria compuesta
}