using ManagerApi.Data;
using ManagerApi.Data.Entities;
using ManagerApi.DTOs.Bugs;
using ManagerApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManagerApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BugsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public BugsController(AppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BugDto>>> GetBugs([FromQuery] Guid? projectId)
    {
        var query = _context.Bugs
            .Include(b => b.Project)
            .Include(b => b.CreatedBy)
            .Include(b => b.AssignedTo)
            .Include(b => b.FixedInTask)
            .AsQueryable();

        if (projectId.HasValue)
        {
            query = query.Where(b => b.ProjectId == projectId.Value);
        }

        return await query
            .Select(b => new BugDto
            {
                Id = b.Id,
                Title = b.Title,
                Description = b.Description,
                Severity = b.Severity,
                Status = b.Status,
                ProjectId = b.ProjectId,
                ProjectName = b.Project.Name,
                CreatedById = b.CreatedById,
                CreatedByName = b.CreatedBy.FullName,
                AssignedToId = b.AssignedToId,
                AssignedToName = b.AssignedTo != null ? b.AssignedTo.FullName : null,
                FixedInTaskId = b.FixedInTaskId,
                FixedInTaskTitle = b.FixedInTask != null ? b.FixedInTask.Title : null,
                CreatedAt = b.CreatedAt
            })
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BugDto>> GetBug(Guid id)
    {
        var bug = await _context.Bugs
            .Include(b => b.Project)
            .Include(b => b.CreatedBy)
            .Include(b => b.AssignedTo)
            .Include(b => b.FixedInTask)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (bug == null)
        {
            return NotFound();
        }

        return new BugDto
        {
            Id = bug.Id,
            Title = bug.Title,
            Description = bug.Description,
            Severity = bug.Severity,
            Status = bug.Status,
            ProjectId = bug.ProjectId,
            ProjectName = bug.Project.Name,
            CreatedById = bug.CreatedById,
            CreatedByName = bug.CreatedBy.FullName,
            AssignedToId = bug.AssignedToId,
            AssignedToName = bug.AssignedTo?.FullName,
            FixedInTaskId = bug.FixedInTaskId,
            FixedInTaskTitle = bug.FixedInTask?.Title,
            CreatedAt = bug.CreatedAt
        };
    }

    [HttpPost]
    public async Task<ActionResult<BugDto>> CreateBug(CreateBugDto model)
    {
        var userId = _currentUserService.GetUserId();

        var bug = new Bug
        {
            Id = Guid.NewGuid(),
            Title = model.Title,
            Description = model.Description,
            Severity = model.Severity ?? "Medium",
            Status = model.Status ?? "New",
            ProjectId = model.ProjectId,
            CreatedById = userId,
            AssignedToId = model.AssignedToId,
            FixedInTaskId = model.FixedInTaskId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Bugs.Add(bug);
        await _context.SaveChangesAsync();

        // Fetch related data
        var project = await _context.Projects.FindAsync(model.ProjectId);
        var creator = await _context.Users.FindAsync(bug.CreatedById);
        var assignee = model.AssignedToId.HasValue ? await _context.Users.FindAsync(model.AssignedToId.Value) : null;
        var fixedInTask = model.FixedInTaskId.HasValue ? await _context.Tasks.FindAsync(model.FixedInTaskId.Value) : null;

        return CreatedAtAction(nameof(GetBug), new { id = bug.Id }, new BugDto
        {
            Id = bug.Id,
            Title = bug.Title,
            Description = bug.Description,
            Severity = bug.Severity,
            Status = bug.Status,
            ProjectId = bug.ProjectId,
            ProjectName = project?.Name ?? "Unknown",
            CreatedById = bug.CreatedById,
            CreatedByName = creator?.FullName ?? "Unknown",
            AssignedToId = bug.AssignedToId,
            AssignedToName = assignee?.FullName,
            FixedInTaskId = bug.FixedInTaskId,
            FixedInTaskTitle = fixedInTask?.Title,
            CreatedAt = bug.CreatedAt
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBug(Guid id, CreateBugDto model)
    {
        var bug = await _context.Bugs.FindAsync(id);

        if (bug == null)
        {
            return NotFound();
        }

        bug.Title = model.Title;
        bug.Description = model.Description;
        bug.Severity = model.Severity;
        bug.Status = model.Status;
        bug.ProjectId = model.ProjectId;
        bug.AssignedToId = model.AssignedToId;
        bug.FixedInTaskId = model.FixedInTaskId;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBug(Guid id)
    {
        var bug = await _context.Bugs.FindAsync(id);
        if (bug == null)
        {
            return NotFound();
        }

        _context.Bugs.Remove(bug);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
