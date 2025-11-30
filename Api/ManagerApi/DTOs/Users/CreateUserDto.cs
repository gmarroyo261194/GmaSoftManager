using System.ComponentModel.DataAnnotations;

namespace ManagerApi.DTOs.Users;

public class CreateUserDto
{
    [Required]
    public string UserName { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required]
    public string FullName { get; set; }

    [Required]
    public string Password { get; set; }
}
