using ManagerApi.Data;
using ManagerApi.Data.Entities;
using ManagerApi.DTOs.Milestones;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManagerApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MilestonesController : ControllerBase
{
    private readonly AppDbContext _context;

    public MilestonesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MilestoneDto>>> GetMilestones([FromQuery] Guid? projectId)
    {
        var query = _context.Milestones.AsQueryable();

        if (projectId.HasValue)
        {
            query = query.Where(m => m.ProjectId == projectId.Value);
        }

        return await query
            .Select(m => new MilestoneDto
            {
                Id = m.Id,
                Name = m.Name,
                TargetDate = m.TargetDate,
                Status = m.Status,
                ProjectId = m.ProjectId
            })
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MilestoneDto>> GetMilestone(Guid id)
    {
        var milestone = await _context.Milestones.FindAsync(id);

        if (milestone == null)
        {
            return NotFound();
        }

        return new MilestoneDto
        {
            Id = milestone.Id,
            Name = milestone.Name,
            TargetDate = milestone.TargetDate,
            Status = milestone.Status,
            ProjectId = milestone.ProjectId
        };
    }

    [HttpPost]
    public async Task<ActionResult<MilestoneDto>> CreateMilestone(CreateMilestoneDto model)
    {
        var milestone = new Milestone
        {
            Id = Guid.NewGuid(),
            Name = model.Name,
            TargetDate = model.TargetDate,
            Status = "Pending",
            ProjectId = model.ProjectId
        };

        _context.Milestones.Add(milestone);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMilestone), new { id = milestone.Id }, new MilestoneDto
        {
            Id = milestone.Id,
            Name = milestone.Name,
            TargetDate = milestone.TargetDate,
            Status = milestone.Status,
            ProjectId = milestone.ProjectId
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMilestone(Guid id, CreateMilestoneDto model)
    {
        var milestone = await _context.Milestones.FindAsync(id);

        if (milestone == null)
        {
            return NotFound();
        }

        milestone.Name = model.Name;
        milestone.TargetDate = model.TargetDate;
        // Status update logic might be separate or included here if DTO supports it.
        // Assuming CreateMilestoneDto doesn't have Status, we might need UpdateMilestoneDto or just ignore it here.
        // But wait, CreateMilestoneDto doesn't have Status. So we can't update status via this endpoint unless we add it.
        // For now, let's assume status is not updated here or we add it to DTO.
        // The user didn't ask for specific status update logic, so I'll leave it as is.
        
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMilestone(Guid id)
    {
        var milestone = await _context.Milestones.FindAsync(id);
        if (milestone == null)
        {
            return NotFound();
        }

        _context.Milestones.Remove(milestone);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
