using ManagerApi.Data;
using ManagerApi.Data.Entities;
using ManagerApi.DTOs.Tasks;
using ManagerApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManagerApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public TasksController(AppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasks([FromQuery] Guid? projectId)
    {
        var query = _context.Tasks
            .Include(t => t.Project)
            .Include(t => t.AssignedTo)
            .AsQueryable();

        if (projectId.HasValue)
        {
            query = query.Where(t => t.ProjectId == projectId.Value);
        }

        return await query
            .Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Status = t.Status,
                Priority = t.Priority,
                DueDate = t.DueDate,
                ProjectId = t.ProjectId,
                ProjectName = t.Project.Name,
                AssignedToId = t.AssignedToId,
                AssignedToName = t.AssignedTo != null ? t.AssignedTo.FullName : null,
                CreatedAt = t.CreatedAt
            })
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskDto>> GetTask(Guid id)
    {
        var task = await _context.Tasks
            .Include(t => t.Project)
            .Include(t => t.AssignedTo)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (task == null)
        {
            return NotFound();
        }

        return new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            DueDate = task.DueDate,
            ProjectId = task.ProjectId,
            ProjectName = task.Project.Name,
            AssignedToId = task.AssignedToId,
            AssignedToName = task.AssignedTo?.FullName,
            CreatedAt = task.CreatedAt
        };
    }

    [HttpPost]
    public async Task<ActionResult<TaskDto>> CreateTask(CreateTaskDto model)
    {
        var userId = _currentUserService.GetUserId();

        if (!model.DueDate.HasValue)
        {
            return BadRequest("DueDate is required.");
        }

        var task = new ProjectTask
        {
            Id = Guid.NewGuid(),
            Title = model.Title,
            Description = model.Description,
            Status = model.Status ?? "Pending",
            Priority = model.Priority ?? "Normal",
            DueDate = model.DueDate.Value,
            ProjectId = model.ProjectId,
            AssignedToId = model.AssignedToId,
            CreatedById = userId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        var project = await _context.Projects.FindAsync(model.ProjectId);
        var assignee = model.AssignedToId.HasValue ? await _context.Users.FindAsync(model.AssignedToId.Value) : null;

        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            DueDate = task.DueDate,
            ProjectId = task.ProjectId,
            ProjectName = project?.Name ?? "Unknown",
            AssignedToId = task.AssignedToId,
            AssignedToName = assignee?.FullName,
            CreatedAt = task.CreatedAt
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(Guid id, CreateTaskDto model)
    {
        var task = await _context.Tasks.FindAsync(id);

        if (task == null)
        {
            return NotFound();
        }

        task.Title = model.Title;
        task.Description = model.Description;
        task.Status = model.Status ?? task.Status;
        task.Priority = model.Priority ?? task.Priority;
        if (model.DueDate.HasValue)
        {
            task.DueDate = model.DueDate.Value;
        }
        task.ProjectId = model.ProjectId;
        task.AssignedToId = model.AssignedToId;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(Guid id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
