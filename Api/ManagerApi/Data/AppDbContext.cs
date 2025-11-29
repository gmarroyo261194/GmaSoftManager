using System.Linq.Expressions;
using ManagerApi.Data.Entities;
using ManagerApi.Data.Interceptors;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    private readonly AuditInterceptor _auditInterceptor;

    public AppDbContext(DbContextOptions<AppDbContext> options, AuditInterceptor auditInterceptor)
        : base(options)
    {
        _auditInterceptor = auditInterceptor;
    }

    // DBSet de todas las entidades
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectMember> ProjectMembers { get; set; }
    public DbSet<ProjectAttachment> ProjectAttachments { get; set; }
    public DbSet<ProjectTask> Tasks { get; set; }
    public DbSet<Milestone> Milestones { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Attachment> Attachments { get; set; }
    public DbSet<Bug> Bugs { get; set; }
    public DbSet<BugComment> BugComments { get; set; }
    public DbSet<BugAttachment> BugAttachments { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<TaskTag> TaskTags { get; set; }
    public DbSet<BugTag> BugTags { get; set; }
    public DbSet<ChangeHistory> ChangeHistories { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<GeneralAttachment> GeneralAttachments { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditInterceptor);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuración de claves compuestas
        modelBuilder.Entity<ProjectMember>().HasKey(pm => new { pm.ProjectId, pm.UserId });
        modelBuilder.Entity<TaskTag>().HasKey(tt => new { tt.TaskId, tt.TagId });
        modelBuilder.Entity<BugTag>().HasKey(bt => new { bt.BugId, bt.TagId });

        // Relaciones adicionales (ejemplo: One-to-Many, Many-to-Many)
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.Roles)
            .HasForeignKey(ur => ur.UserId);

        modelBuilder.Entity<ProjectMember>()
            .HasOne(pm => pm.User)
            .WithMany(u => u.ProjectMembers)
            .HasForeignKey(pm => pm.UserId);
        modelBuilder.Entity<ProjectMember>()
            .HasOne(pm => pm.Project)
            .WithMany(p => p.Members)
            .HasForeignKey(pm => pm.ProjectId);

        modelBuilder.Entity<ProjectAttachment>()
            .HasOne(pa => pa.Project)
            .WithMany(p => p.Attachments)
            .HasForeignKey(pa => pa.ProjectId);

        modelBuilder.Entity<ProjectTask>()
            .HasOne(t => t.Project)
            .WithMany(p => p.Tasks)
            .HasForeignKey(t => t.ProjectId);
        modelBuilder.Entity<ProjectTask>()
            .HasOne(t => t.AssignedTo)
            .WithMany()
            .HasForeignKey(t => t.AssignedToId);

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Task)
            .WithMany(t => t.Comments)
            .HasForeignKey(c => c.TaskId);
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany(u => u.TaskComments)
            .HasForeignKey(c => c.UserId);

        modelBuilder.Entity<Attachment>()
            .HasOne(a => a.Task)
            .WithMany(t => t.Attachments)
            .HasForeignKey(a => a.TaskId);

        modelBuilder.Entity<Milestone>()
            .HasOne(m => m.Project)
            .WithMany(p => p.Milestones)
            .HasForeignKey(m => m.ProjectId);

        modelBuilder.Entity<Bug>()
            .HasOne(b => b.Project)
            .WithMany(p => p.Bugs)
            .HasForeignKey(b => b.ProjectId);
        modelBuilder.Entity<Bug>()
            .HasOne(b => b.CreatedBy)
            .WithMany()
            .HasForeignKey(b => b.CreatedById);
        modelBuilder.Entity<Bug>()
            .HasOne(b => b.AssignedTo)
            .WithMany()
            .HasForeignKey(b => b.AssignedToId);
        modelBuilder.Entity<Bug>()
            .HasOne(b => b.FixedInTask)
            .WithMany(t => t.FixedBugs)
            .HasForeignKey(b => b.FixedInTaskId);

        modelBuilder.Entity<BugComment>()
            .HasOne(bc => bc.Bug)
            .WithMany(b => b.Comments)
            .HasForeignKey(bc => bc.BugId);
        modelBuilder.Entity<BugComment>()
            .HasOne(bc => bc.User)
            .WithMany(u => u.BugComments)
            .HasForeignKey(bc => bc.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<BugAttachment>()
            .HasOne(ba => ba.Bug)
            .WithMany(b => b.Attachments)
            .HasForeignKey(ba => ba.BugId);

        modelBuilder.Entity<TaskTag>()
            .HasOne(tt => tt.Task)
            .WithMany(t => t.Tags)
            .HasForeignKey(tt => tt.TaskId);
        modelBuilder.Entity<TaskTag>()
            .HasOne(tt => tt.Tag)
            .WithMany(tg => tg.TaskTags)
            .HasForeignKey(tt => tt.TagId);

        modelBuilder.Entity<BugTag>()
            .HasOne(bt => bt.Bug)
            .WithMany(b => b.Tags)
            .HasForeignKey(bt => bt.BugId);
        modelBuilder.Entity<BugTag>()
            .HasOne(bt => bt.Tag)
            .WithMany(t => t.BugTags)
            .HasForeignKey(bt => bt.TagId);

        modelBuilder.Entity<ChangeHistory>()
            .HasOne(ch => ch.ChangedBy)
            .WithMany(u => u.ChangeHistories)
            .HasForeignKey(ch => ch.ChangedById);

        modelBuilder.Entity<Notification>()
            .HasOne(n => n.User)
            .WithMany(u => u.Notifications)
            .HasForeignKey(n => n.UserId);

        modelBuilder.Entity<GeneralAttachment>()
            .HasOne(ga => ga.UploadedBy)
            .WithMany(u => u.UploadedAttachments)
            .HasForeignKey(ga => ga.UploadedById);

        modelBuilder.Entity<RefreshToken>()
            .HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId);

        // Filtro global para soft delete (solo para AuditEntity)
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(AuditEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .HasQueryFilter(GetIsDeletedRestriction(entityType.ClrType));
            }
        }
    }

    // Helper para crear expresión de filtro
    private static LambdaExpression GetIsDeletedRestriction(Type type)
    {
        var parameter = Expression.Parameter(type, "e");
        var property = Expression.Property(parameter, nameof(AuditEntity.IsDeleted));
        var condition = Expression.Equal(property, Expression.Constant(false));
        return Expression.Lambda(condition, parameter);
    }
}