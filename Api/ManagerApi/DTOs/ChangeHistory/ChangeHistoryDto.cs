namespace ManagerApi.DTOs.ChangeHistory;

public class ChangeHistoryDto
{
    public Guid Id { get; set; }
    public string EntityType { get; set; }
    public Guid EntityId { get; set; }
    public Guid ChangedById { get; set; }
    public string ChangedByName { get; set; }
    public string ChangeType { get; set; }
    public string ChangeDescription { get; set; }
    public DateTime ChangedAt { get; set; }
}
