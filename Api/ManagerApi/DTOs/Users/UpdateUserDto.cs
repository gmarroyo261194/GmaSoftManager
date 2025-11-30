using System.ComponentModel.DataAnnotations;

namespace ManagerApi.DTOs.Users;

public class UpdateUserDto
{
    [Required]
    public string FullName { get; set; }

    public bool IsActive { get; set; }
}
