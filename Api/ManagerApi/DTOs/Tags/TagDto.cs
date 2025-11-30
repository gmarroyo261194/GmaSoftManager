using System.ComponentModel.DataAnnotations;

namespace ManagerApi.DTOs.Tags;

public class TagDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}

public class CreateTagDto
{
    [Required, StringLength(100)]
    public string Name { get; set; }
}
