using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Quark_Backend.Models;
using Quark_Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Quark_Backend.DAL;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

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
    public IActionResult Check(string email)
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
    public async Task<IActionResult> Search(string? firstName, string? lastName, string? jobPosition, string? department, string? email)
    {
        List<User> searchedUsers;
        if(firstName is null && lastName is null && jobPosition is null && department is null && email is null)
        {
            return Ok("Too few information about users to search for them."); //should be Ok response?
        }
        if(jobPosition is null)//for now applying only null arguments will return all users
        {
            searchedUsers = await _context.Users
                .Where(u => firstName == null || u.FirstName == firstName)
                .Where(u => lastName == null || u.LastName == lastName)
                .Where(u => department == null || u.JobPosition.Department.Equals(department))
                .Where(u => email == null ||  u.Email == email)//remove in future
                .ToListAsync();
        }
        else
        {
            searchedUsers = await _context.Users
                .Where(u => firstName == null || u.FirstName == firstName)
                .Where(u => lastName == null || u.LastName == lastName)
                .Where(u => u.JobPosition.Equals(jobPosition))
                .ToListAsync();
        }
        return Ok(searchedUsers);
    }
    // for testing purposes only - to be removed in future
    [HttpPost]
    public async Task<IActionResult> Clear()
    {
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
        //'@quark.com' has 10 letters; email column has max length 30
        string pattern = @"^([A-Z]?|[a-z])[a-z]{0,19}\.([A-Z]?|[a-z])[a-z]{0,19}@quark\.com";
        
        if(Regex.IsMatch(email, pattern) == false)//check email format
        {
            return BadRequest("Email has wrong format.");
        }
        User user = new User{Email=email};
        _context.Add(user);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
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
        return StatusCode(StatusCodes.Status201Created, user);
    }    

}
