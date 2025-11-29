using System.ComponentModel.DataAnnotations;

namespace ManagerApi.Data.Entities;

public class TaskTag
{
    [Required]
    public Guid TaskId { get; set; }
    [Required]
    public Guid TagId { get; set; }

    public ProjectTask Task { get; set; }
    public Tag Tag { get; set; }

    // Clave primaria compuesta
}