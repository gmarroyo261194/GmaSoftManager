using System.ComponentModel.DataAnnotations;

namespace ManagerApi.DTOs.Comments;

public class CreateCommentDto
{
    [Required]
    public string Content { get; set; }

    [Required]
    public Guid TaskId { get; set; }
}
