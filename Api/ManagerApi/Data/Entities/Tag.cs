using System.ComponentModel.DataAnnotations;

namespace ManagerApi.Data.Entities;

public class Tag
{
    [Key]
    public Guid Id { get; set; }
    [Required, StringLength(100)]
    public string Name { get; set; }

    public ICollection<TaskTag> TaskTags { get; set; }
    public ICollection<BugTag> BugTags { get; set; }
}