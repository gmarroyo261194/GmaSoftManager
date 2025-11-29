namespace ManagerApi.Data.Entities;

public class AuditEntity
{
    // Usuario que creó (insert)
    public Guid CreatedById { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Usuario que modificó (update)
    public Guid? ModifiedById { get; set; }
    public DateTime? ModifiedAt { get; set; }

    // Usuario que eliminó (delete lógico, si aplica)
    public Guid? DeletedById { get; set; }
    public DateTime? DeletedAt { get; set; }

    // Estado de borrado lógico
    public bool IsDeleted { get; set; } = false;
}