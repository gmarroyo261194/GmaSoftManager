using System.ComponentModel.DataAnnotations;

namespace ManagerApi.DTOs.Projects;

public class CreateProjectDto
{
    [Required, StringLength(100)]
    public string Name { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EstimatedEndDate { get; set; }
}
