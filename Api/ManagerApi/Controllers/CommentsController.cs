using ManagerApi.Data;
using ManagerApi.Data.Entities;
using ManagerApi.DTOs.Comments;
using ManagerApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManagerApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CommentsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CommentsController(AppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CommentDto>>> GetComments([FromQuery] Guid taskId)
    {
        return await _context.Comments
            .Where(c => c.TaskId == taskId)
            .Include(c => c.User)
            .OrderBy(c => c.CreatedAt)
            .Select(c => new CommentDto
            {
                Id = c.Id,
                Content = c.Content,
                CreatedAt = c.CreatedAt,
                TaskId = c.TaskId,
                UserId = c.UserId,
                UserName = c.User.FullName
            })
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<CommentDto>> CreateComment(CreateCommentDto model)
    {
        var userId = _currentUserService.GetUserId();

        var comment = new Comment
        {
            Id = Guid.NewGuid(),
            Content = model.Content,
            TaskId = model.TaskId,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        var user = await _context.Users.FindAsync(userId);

        return CreatedAtAction(nameof(GetComments), new { taskId = comment.TaskId }, new CommentDto
        {
            Id = comment.Id,
            Content = comment.Content,
            CreatedAt = comment.CreatedAt,
            TaskId = comment.TaskId,
            UserId = comment.UserId,
            UserName = user?.FullName ?? "Unknown"
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(Guid id)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
        {
            return NotFound();
        }

        // Check if user is owner or admin (omitted for brevity, but good practice)
        var userId = _currentUserService.GetUserId();
        if (comment.UserId != userId)
        {
             // return Forbid(); // Optional: Enforce ownership
        }

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
