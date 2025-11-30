using ManagerApi.Data;
using ManagerApi.Data.Entities;
using ManagerApi.DTOs.UserRoles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManagerApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserRolesController : ControllerBase
{
    private readonly AppDbContext _context;

    public UserRolesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<IEnumerable<UserRoleDto>>> GetUserRoles(Guid userId)
    {
        return await _context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Select(ur => new UserRoleDto
            {
                Id = ur.Id,
                UserId = ur.UserId,
                Role = ur.Role
            })
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<UserRoleDto>> AddUserRole(AddUserRoleDto model)
    {
        var user = await _context.Users.FindAsync(model.UserId);
        if (user == null) return NotFound("User not found");

        var userRole = new UserRole
        {
            Id = Guid.NewGuid(),
            UserId = model.UserId,
            Role = model.Role
        };

        _context.UserRoles.Add(userRole);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUserRoles), new { userId = model.UserId }, new UserRoleDto
        {
            Id = userRole.Id,
            UserId = userRole.UserId,
            Role = userRole.Role
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveUserRole(Guid id)
    {
        var userRole = await _context.UserRoles.FindAsync(id);
        if (userRole == null) return NotFound();

        _context.UserRoles.Remove(userRole);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
