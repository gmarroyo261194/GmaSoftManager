using ManagerApi.Data.Entities;
using ManagerApi.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

// Interceptor para auditoría
namespace ManagerApi.Data.Interceptors;

public class AuditInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserService _userService; // Servicio para identificar usuario actual

    public AuditInterceptor(ICurrentUserService userService)
    {
        _userService = userService;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;

        var username = _userService.GetUserId(); // Debe devolver el usuario actual (Guid)

        foreach (var entry in context.ChangeTracker.Entries<AuditEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.CreatedById = username;
                entry.Entity.IsDeleted = false;

                entry.Entity.ModifiedById = null;
                entry.Entity.ModifiedAt = null;
                entry.Entity.DeletedById = null;
                entry.Entity.DeletedAt = null;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.ModifiedAt = DateTime.UtcNow;
                entry.Entity.ModifiedById = username;
            }
            else if (entry.State == EntityState.Deleted)
            {
                // Soft delete
                entry.State = EntityState.Modified;
                entry.Entity.DeletedAt = DateTime.UtcNow;
                entry.Entity.DeletedById = username;
                entry.Entity.IsDeleted = true;
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}