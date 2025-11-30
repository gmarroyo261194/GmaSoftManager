using System.ComponentModel.DataAnnotations;

namespace ManagerApi.DTOs.ProjectMembers;

public class ProjectMemberDto
{
    public Guid ProjectId { get; set; }
    public string ProjectName { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public string Role { get; set; }
}

public class AddProjectMemberDto
{
    [Required]
    public Guid ProjectId { get; set; }
    [Required]
    public Guid UserId { get; set; }
    [Required, StringLength(50)]
    public string Role { get; set; }
}
