using System.ComponentModel.DataAnnotations;
using ManagerApi.Data.Entities;

namespace ManagerApi.DTOs.Bugs;

public class CreateBugDto
{
    [Required, StringLength(200)]
    public string Title { get; set; }

    public string Description { get; set; }

    public string Severity { get; set; }
    public string Status { get; set; }

    [Required]
    public Guid ProjectId { get; set; }

    public Guid? AssignedToId { get; set; }
    public Guid? FixedInTaskId { get; set; }
}
