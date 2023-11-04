using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Quark_Backend.Models;
using Quark_Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Quark_Backend.DAL;

namespace Quark_Backend.Controllers;



[ApiController]
[Route("api/[controller]/[action]")]
public class UsersController : ControllerBase
{
    private readonly QuarkDbContext _context;

    public UsersController(QuarkDbContext context)
    {
        _context = context;
    }

    private bool UserExists(int id)
    {
        return _context.Users.Any(u => u.Id == id);
    }

    [HttpPut]
    public async Task<IActionResult> Add(int id, User user)
    {
        if (id != user.Id)
        {
            return BadRequest();
        }
        _context.Add(user);
        // _context.Entry(user).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }    

}
