namespace ManagerApi.DTOs.Notifications;

public class NotificationDto
{
    public Guid Id { get; set; }
    public string NotificationType { get; set; }
    public string EntityType { get; set; }
    public Guid EntityId { get; set; }
    public string Message { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}
