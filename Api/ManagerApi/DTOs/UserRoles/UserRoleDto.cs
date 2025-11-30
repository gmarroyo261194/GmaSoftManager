using System.ComponentModel.DataAnnotations;

namespace ManagerApi.DTOs.UserRoles;

public class UserRoleDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Role { get; set; }
}

public class AddUserRoleDto
{
    [Required]
    public Guid UserId { get; set; }
    [Required, StringLength(50)]
    public string Role { get; set; }
}
