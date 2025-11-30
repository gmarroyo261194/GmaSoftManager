using ManagerApi.Data;
using ManagerApi.Data.Entities;
using ManagerApi.DTOs.Tags;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManagerApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TagsController : ControllerBase
{
    private readonly AppDbContext _context;

    public TagsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TagDto>>> GetTags()
    {
        return await _context.Tags
            .Select(t => new TagDto { Id = t.Id, Name = t.Name })
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<TagDto>> CreateTag(CreateTagDto model)
    {
        var tag = new Tag { Id = Guid.NewGuid(), Name = model.Name };
        _context.Tags.Add(tag);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTags), new { id = tag.Id }, new TagDto { Id = tag.Id, Name = tag.Name });
    }

    [HttpPost("Task/{taskId}")]
    public async Task<IActionResult> AddTagToTask(Guid taskId, [FromBody] Guid tagId)
    {
        var task = await _context.Tasks.FindAsync(taskId);
        if (task == null) return NotFound("Task not found");
        
        var tag = await _context.Tags.FindAsync(tagId);
        if (tag == null) return NotFound("Tag not found");

        var taskTag = new TaskTag { TaskId = taskId, TagId = tagId };
        _context.TaskTags.Add(taskTag);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("Bug/{bugId}")]
    public async Task<IActionResult> AddTagToBug(Guid bugId, [FromBody] Guid tagId)
    {
        var bug = await _context.Bugs.FindAsync(bugId);
        if (bug == null) return NotFound("Bug not found");
        
        var tag = await _context.Tags.FindAsync(tagId);
        if (tag == null) return NotFound("Tag not found");

        var bugTag = new BugTag { BugId = bugId, TagId = tagId };
        _context.BugTags.Add(bugTag);
        await _context.SaveChangesAsync();

        return Ok();
    }
}
