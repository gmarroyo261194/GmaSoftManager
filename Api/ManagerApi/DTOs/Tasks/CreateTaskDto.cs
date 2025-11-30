using System.ComponentModel.DataAnnotations;
using ManagerApi.Data.Entities;

namespace ManagerApi.DTOs.Tasks;

public class CreateTaskDto
{
    [Required, StringLength(200)]
    public string Title { get; set; }

    public string Description { get; set; }

    public string Status { get; set; }
    public string Priority { get; set; }
    public DateTime? DueDate { get; set; }

    [Required]
    public Guid ProjectId { get; set; }

    public Guid? AssignedToId { get; set; }
}
