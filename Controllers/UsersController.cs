using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Quark_Backend.Models;
using Quark_Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Quark_Backend.DAL;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Data;

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
    private bool UserExists(string email)
    {
        return _context.Users.Any(u => u.Email == email);
    }

    [HttpGet]
    public async Task<IActionResult> Check(string email)
    {
        if(UserExists(email))
        {
            return Ok("User exists.");
        }
        else
        {
            return Ok("User doesn't exist.");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Search(string? firstName, string? lastName, string? jobPosition, string? department, string? email) //properties to include; email to be removed in future
    {
        IQueryable<User> searchedUsers;
        if(firstName is null && lastName is null && jobPosition is null && department is null && email is null)
        {
            return Ok("Too few information about users to search for them."); //should be Ok response?
        }
        if(jobPosition is null)
        {
            searchedUsers = _context.Users // is there a check against null? --------------------------------------
                .Where(u => u.FirstName == firstName)
                .Where(u => u.LastName == lastName)
                .Where(u => u.Job.Department.Equals(department)).Where(u => u.Email == email);
        }
        else
        {
            searchedUsers = _context.Users
                .Where(u => u.FirstName == firstName)
                .Where(u => u.LastName == lastName)
                .Where(u => u.Job.Equals(jobPosition));
        }
        try 
        {
            await _context.SaveChangesAsync();
        }
        catch 
        {
            throw;
        }
        return Ok(searchedUsers);
    }
    // for testing purposes only - to be removed in future
    [HttpPost]
    public async Task<IActionResult> Clear()
    {
        _context.Users.FromSql($"TRUNCATE [userTable] RESTART IDENTITY;");
        _context.Users.RemoveRange(_context.Users);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw;
        }
        return Ok("All users were deleted.");
    }

    [HttpPut]
    public async Task<IActionResult> Register(string email)
    {

        _context.Add(new User{Email=email});
        // _context.Entry(user).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(email))
            {
                return NotFound();
            }
            else
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }
        }
        return NoContent();
    }    

}
