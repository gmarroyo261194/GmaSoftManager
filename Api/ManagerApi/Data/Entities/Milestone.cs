using System.ComponentModel.DataAnnotations;

namespace ManagerApi.Data.Entities;

public class Milestone
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public Guid ProjectId { get; set; }
    [Required, StringLength(200)]
    public string Name { get; set; }
    [Required]
    public DateTime TargetDate { get; set; }
    [Required, StringLength(30)]
    public string Status { get; set; } // Pendiente, conseguido

    public Project Project { get; set; }
}