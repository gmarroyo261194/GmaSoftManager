using ManagerApi.Data;
using ManagerApi.Data.Entities;
using ManagerApi.DTOs.Attachments;
using ManagerApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManagerApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AttachmentsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public AttachmentsController(AppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    [HttpPost("Task/{taskId}")]
    public async Task<ActionResult<AttachmentDto>> UploadTaskAttachment(Guid taskId, [FromBody] CreateAttachmentDto model)
    {
        var task = await _context.Tasks.FindAsync(taskId);
        if (task == null) return NotFound("Task not found");

        var attachment = new Attachment
        {
            Id = Guid.NewGuid(),
            TaskId = taskId,
            FileName = model.FileName,
            FilePath = model.FilePath,
            UploadedAt = DateTime.UtcNow,
            CreatedById = _currentUserService.GetUserId(),
            CreatedAt = DateTime.UtcNow
        };

        _context.Attachments.Add(attachment);
        await _context.SaveChangesAsync();

        return Ok(new AttachmentDto
        {
            Id = attachment.Id,
            FileName = attachment.FileName,
            FilePath = attachment.FilePath,
            UploadedAt = attachment.UploadedAt,
            EntityId = taskId,
            EntityType = "Task"
        });
    }

    [HttpPost("Bug/{bugId}")]
    public async Task<ActionResult<AttachmentDto>> UploadBugAttachment(Guid bugId, [FromBody] CreateAttachmentDto model)
    {
        var bug = await _context.Bugs.FindAsync(bugId);
        if (bug == null) return NotFound("Bug not found");

        var attachment = new BugAttachment
        {
            Id = Guid.NewGuid(),
            BugId = bugId,
            FileName = model.FileName,
            FilePath = model.FilePath,
            UploadedAt = DateTime.UtcNow
        };

        _context.BugAttachments.Add(attachment);
        await _context.SaveChangesAsync();

        return Ok(new AttachmentDto
        {
            Id = attachment.Id,
            FileName = attachment.FileName,
            FilePath = attachment.FilePath,
            UploadedAt = attachment.UploadedAt,
            EntityId = bugId,
            EntityType = "Bug"
        });
    }

    [HttpPost("Project/{projectId}")]
    public async Task<ActionResult<AttachmentDto>> UploadProjectAttachment(Guid projectId, [FromBody] CreateAttachmentDto model)
    {
        var project = await _context.Projects.FindAsync(projectId);
        if (project == null) return NotFound("Project not found");

        var attachment = new ProjectAttachment
        {
            Id = Guid.NewGuid(),
            ProjectId = projectId,
            FileName = model.FileName,
            FilePath = model.FilePath,
            UploadedAt = DateTime.UtcNow,
            CreatedById = _currentUserService.GetUserId(),
            CreatedAt = DateTime.UtcNow
        };

        _context.ProjectAttachments.Add(attachment);
        await _context.SaveChangesAsync();

        return Ok(new AttachmentDto
        {
            Id = attachment.Id,
            FileName = attachment.FileName,
            FilePath = attachment.FilePath,
            UploadedAt = attachment.UploadedAt,
            EntityId = projectId,
            EntityType = "Project"
        });
    }
}
