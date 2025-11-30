using ManagerApi.Data;
using ManagerApi.Data.Entities;
using ManagerApi.DTOs.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace ManagerApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        return await _context.Users
            .Select(u => new UserDto
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                FullName = u.FullName,
                IsActive = u.IsActive
            })
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(Guid id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return new UserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            FullName = user.FullName,
            IsActive = user.IsActive
        };
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto model)
    {
        if (await _context.Users.AnyAsync(u => u.Email == model.Email))
        {
            return BadRequest("Email already exists");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = model.UserName,
            Email = model.Email,
            FullName = model.FullName,
            PasswordHash = HashPassword(model.Password),
            IsActive = true
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, new UserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            FullName = user.FullName,
            IsActive = user.IsActive
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, UpdateUserDto model)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        user.FullName = model.FullName;
        user.IsActive = model.IsActive;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        // Soft delete logic could be here, but for now we'll just deactivate
        // Or if we want to delete:
        // _context.Users.Remove(user);
        
        // Let's just deactivate for safety as per standard enterprise apps
        user.IsActive = false;
        
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
}
