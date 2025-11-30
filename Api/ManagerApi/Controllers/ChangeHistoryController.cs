using ManagerApi.Data;
using ManagerApi.DTOs.ChangeHistory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManagerApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ChangeHistoryController : ControllerBase
{
    private readonly AppDbContext _context;

    public ChangeHistoryController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ChangeHistoryDto>>> GetChangeHistory([FromQuery] string entityType, [FromQuery] Guid entityId)
    {
        return await _context.ChangeHistories
            .Where(ch => ch.EntityType == entityType && ch.EntityId == entityId)
            .Include(ch => ch.ChangedBy)
            .OrderByDescending(ch => ch.ChangedAt)
            .Select(ch => new ChangeHistoryDto
            {
                Id = ch.Id,
                EntityType = ch.EntityType,
                EntityId = ch.EntityId,
                ChangedById = ch.ChangedById,
                ChangedByName = ch.ChangedBy.FullName,
                ChangeType = ch.ChangeType,
                ChangeDescription = ch.ChangeDescription,
                ChangedAt = ch.ChangedAt
            })
            .ToListAsync();
    }
}
