using System.ComponentModel.DataAnnotations;

namespace ManagerApi.Data.Entities;

public class User
{
    [Key]
    public Guid Id { get; set; }

    [Required, StringLength(100)]
    public string UserName { get; set; }

    [Required, StringLength(150)]
    public string Email { get; set; }

    [Required, StringLength(150)]
    public string FullName { get; set; }

    [Required]
    public string PasswordHash { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation
    public ICollection<UserRole> Roles { get; set; }
    public ICollection<ProjectMember> ProjectMembers { get; set; }
    public ICollection<Comment> TaskComments { get; set; }
    public ICollection<BugComment> BugComments { get; set; }
    public ICollection<ChangeHistory> ChangeHistories { get; set; }
    public ICollection<Notification> Notifications { get; set; }
    public ICollection<GeneralAttachment> UploadedAttachments { get; set; }
    public ICollection<RefreshToken> RefreshTokens { get; set; }
}