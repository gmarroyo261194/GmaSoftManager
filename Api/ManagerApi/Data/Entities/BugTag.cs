using System.ComponentModel.DataAnnotations;

namespace ManagerApi.Data.Entities;

public class BugTag
{
    [Required]
    public Guid BugId { get; set; }
    [Required]
    public Guid TagId { get; set; }

    public Bug Bug { get; set; }
    public Tag Tag { get; set; }

    // Clave primaria compuesta
}