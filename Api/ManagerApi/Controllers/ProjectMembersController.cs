using ManagerApi.Data;
using ManagerApi.Data.Entities;
using ManagerApi.DTOs.ProjectMembers;
using ManagerApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManagerApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProjectMembersController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public ProjectMembersController(AppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    [HttpGet("{projectId}")]
    public async Task<ActionResult<IEnumerable<ProjectMemberDto>>> GetProjectMembers(Guid projectId)
    {
        return await _context.ProjectMembers
            .Where(pm => pm.ProjectId == projectId)
            .Include(pm => pm.User)
            .Include(pm => pm.Project)
            .Select(pm => new ProjectMemberDto
            {
                ProjectId = pm.ProjectId,
                ProjectName = pm.Project.Name,
                UserId = pm.UserId,
                UserName = pm.User.FullName,
                Role = pm.Role
            })
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult> AddProjectMember(AddProjectMemberDto model)
    {
        var project = await _context.Projects.FindAsync(model.ProjectId);
        if (project == null) return NotFound("Project not found");

        var user = await _context.Users.FindAsync(model.UserId);
        if (user == null) return NotFound("User not found");

        var member = new ProjectMember
        {
            ProjectId = model.ProjectId,
            UserId = model.UserId,
            Role = model.Role,
            CreatedById = _currentUserService.GetUserId(),
            CreatedAt = DateTime.UtcNow
        };

        _context.ProjectMembers.Add(member);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("{projectId}/{userId}")]
    public async Task<IActionResult> RemoveProjectMember(Guid projectId, Guid userId)
    {
        var member = await _context.ProjectMembers
            .FirstOrDefaultAsync(pm => pm.ProjectId == projectId && pm.UserId == userId);

        if (member == null) return NotFound();

        _context.ProjectMembers.Remove(member);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
