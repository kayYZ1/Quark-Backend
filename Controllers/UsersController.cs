using Quark_Backend.DAL;
using Quark_Backend.Models;
using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Quark_Backend.Entities;
using Microsoft.EntityFrameworkCore;

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

    static string GenerateUsername(string firstName, string lastName)
    {
        string firstPart = firstName.Substring(0, Math.Min(3, firstName.Length));
        string lastPart = lastName.Substring(Math.Max(0, lastName.Length - 3));

        return firstPart + lastPart;
    }

    [HttpGet]
    public IActionResult Check(string email)
    {
        if (UserExists(email))
        {
            return Ok("User exists.");
        }
        else
        {
            return Ok("User doesn't exist.");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Search(string searchPhrase)
    {
        List<User> users;
        var filteredUsers = new List<User>();
        string pattern;
        string[] keywords = searchPhrase.Split(' ');
        users = await _context.Users.ToListAsync();
        if(keywords.Length == 1) //searching for "usernames" that have form of concatenated not-full names and surnames - which also guaranties that if the only keyword is complete name of user, that user will be included
        {
            RegexOptions options = RegexOptions.IgnoreCase;
            pattern = $"{keywords.First()}[a-z]*";
            foreach (var user in users)
            {
                if(user.Username == null) continue;
                var username = user.Username;
                if(Regex.IsMatch(username, pattern, options))
                    filteredUsers.Add(user);
            }
        }
        //2 or more keywords
        //assumption that if there is more than one keyword, only the last is treated like it can be "incomplete"
        /* planned behaviour: 
        case1:
            searchPhrase: adam wa
            potentially searched users:
                adam waleń
                adam wachlarz
        case2:
            searchPhrase: ada wa
            potentially searched users:
                ada wawrzyńska
                ada wartownik
        case3:
            searchedString: ada
            potentially searched users:
                adawal423 (adam waleń)
                adawac122 (adam wachlarz)
                adawaw345 (ada wawrzyńska)
                adawar294 (ada wartownik)
        */
        //combinations: treat 1st keyword as firstName and surname and second too
        


        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> DetailedSearch(string? firstName, string? lastName, string? jobPosition, string? department, string? email)
    {
        List<User> searchedUsers;
        if (firstName is null && lastName is null && jobPosition is null && department is null && email is null)
        {
            return Ok("Too few information about users to search for them."); //should be Ok response?
        }
        if (jobPosition is null)//for now applying only null arguments will return all users
        {
            searchedUsers = await _context.Users
                .Where(u => firstName == null || u.FirstName == firstName)
                .Where(u => lastName == null || u.LastName == lastName)
                .Where(u => department == null || u.JobPosition.Department.Equals(department))
                .Where(u => email == null || u.Email == email)//remove in future
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
    public async Task<IActionResult> UpdateProfile([FromBody] UserInfoModel userData)
    {
        var _user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userData.Email);
        if (_user == null)
        {
            return Unauthorized("User not found");
        }
        _user.FirstName = userData.FirstName;
        _user.LastName = userData.LastName;
        _user.SelfDescription = userData.SelfDescription;
        _user.PictureUrl = userData.PictureUrl;
        _user.Username = GenerateUsername(_user.FirstName, _user.LastName);
        var user = new
        {
            _user.Email,
            _user.FirstName,
            _user.LastName,
            _user.Username,
            _user.SelfDescription,
            _user.PictureUrl
        };
        try
        {
            await _context.SaveChangesAsync();
            return Ok(user);
        }
        catch (DbUpdateException)
        {
            return BadRequest("Update error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] UserRegistrationModel userData)
    {
        //'@quark.com' has 10 letters; email column has max length 30
        string pattern = @"^([A-Z]?|[a-z])[a-z]{0,19}\.([A-Z]?|[a-z])[a-z]{0,19}@gmail\.com";

        if (Regex.IsMatch(userData.Email, pattern) == false)//check email format
        {
            return BadRequest("Email has wrong format.");
        }
        User user = new User { Email = userData.Email, Password = userData.Password };
        _context.Add(user);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            if (!UserExists(userData.Email))
            {
                return NotFound();
            }
            else
            {
                return BadRequest("Email already exist");
            }
        }
        return Ok(user);
    }

}
