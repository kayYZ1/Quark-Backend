using System.Collections;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Quark_Backend.DAL;
using Quark_Backend.Entities;
using Quark_Backend.Models;

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
    public string GenerateUsername(string firstName, string lastName) //in future remove [HttpGet] and make private
    {
        string firstPart = firstName.Substring(0, Math.Min(3, firstName.Length));
        string lastPart = lastName.Substring(0, Math.Min(3, lastName.Length));

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
            <by name>
                adam (wawrzyński)
            <by surname>
                adamczyk [krzysztof]
            <by username>
                adawal423 (adam waleń)
                adawac122 (adam wachlarz)
                adawaw345 (ada wawrzyńska)
                adawar294 (ada wartownik)
        */
        List<User> users;
        var filteredUsers = new List<User>();
        string[] keywords = searchPhrase.Split(' ');
        users = await _context.Users.ToListAsync();
        RegexOptions options = RegexOptions.IgnoreCase;
        if (keywords.Length == 1) //only case where there is searching for username
        {
            var keyword = keywords.First();
            if (keyword.Length <= 3)
            {
                string pattern = $"{keyword}[a-z]*";
                foreach (var user in users)
                {
                    if (
                        Regex.IsMatch(user.Username, pattern, options)
                        || Regex.IsMatch(user.LastName, pattern, options)
                    )
                        filteredUsers.Add(user);
                }
            }
            else // > 3
            {
                string pattern = $"{keyword}[a-z]*";
                foreach (var user in users)
                {
                    int loginFirstPartLength = 3; //use elsewhere in future?
                    int loginSecondPartLength = 3;
                    bool shouldMatchAgainstFirstName = true;
                    bool shouldMatchAgainstLastName = true;
                    if (user.Username.Length < 6)
                    {
                        if (user.FirstName.Length < 3)
                        {
                            loginFirstPartLength = user.FirstName.Length;
                            if (keyword.Length > loginFirstPartLength) //change to keyword.Length > user.FirstName.Length?
                                shouldMatchAgainstFirstName = false;
                        }
                        if (user.LastName.Length < 3)
                        {
                            loginSecondPartLength = user.LastName.Length;
                            if (keyword.Length > loginSecondPartLength)
                                shouldMatchAgainstLastName = false;
                        }
                    }
                    if (
                        Regex.IsMatch(user.Username, pattern, options)
                        || shouldMatchAgainstFirstName
                            && Regex.IsMatch(user.FirstName, pattern, options)
                        || shouldMatchAgainstLastName
                            && Regex.IsMatch(user.LastName, pattern, options)
                    )
                    {
                        filteredUsers.Add(user);
                    }
                }
            }
        }
        else // keywords.Length > 1
        {
            /*
            assumption that if there is more than one keyword, only the last is treated like it can be "incomplete". Searching for usernames is ommited, because if there is second keyword it means that first is either first name or last name and so second shouldn't be username because username alone is sufficient to search for user by both first name and last name (because username contains part of name and surname)
            */
            Array.Resize(ref keywords, 2);
            string pattern = $"{keywords[0]} {keywords[1]}[a-z]*";
            foreach (var user in users)
            {
                // if(keywords[0] == user.FirstName) ;
                var stringToMatchA = $"{user.FirstName} {user.LastName}"; //combinations
                var stringToMatchB = $"{user.LastName} {user.FirstName}";
                if (
                    Regex.IsMatch(stringToMatchA, pattern, options)
                    || Regex.IsMatch(stringToMatchB, pattern, options)
                )
                {
                    filteredUsers.Add(user);
                }
            }
        }
        return Ok(filteredUsers);
    }

    [HttpGet]
    public async Task<IActionResult> DetailedSearch(
        string? firstName,
        string? lastName,
        string? jobPosition,
        string? department,
        string? email
    )
    {
        List<User> searchedUsers;
        if (
            firstName is null
            && lastName is null
            && jobPosition is null
            && department is null
            && email is null
        )
        {
            return Ok("Too few information about users to search for them."); //should be Ok response?
        }
        if (jobPosition is null) //if searched feature (like firstName) is null then it does not filter by this feature
        {
            searchedUsers = await _context
                .Users
                .Where(u => firstName == null || u.FirstName == firstName)
                .Where(u => lastName == null || u.LastName == lastName)
                .Where(u => department == null || u.JobPosition.Department.Equals(department))
                .Where(u => email == null || u.Email == email) //remove in future
                .ToListAsync();
        }
        else
        {
            searchedUsers = await _context
                .Users
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

    [HttpGet]
    public async Task<ActionResult<List<object>>> GetUsers()
    {
        List<User> _users = await _context.Users.ToListAsync();
        var users = new List<object>();

        foreach (User user in _users)
        {
            var data = new
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PictureUrl = user.PictureUrl
            };

            users.Add(data);
        }
        return users;
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromBody] UserInfoModel userData)
    {
        //problem if two departments have job with the same name; could be fixed if department name would be included in UserInfoModel
<<<<<<< HEAD
        var jobReference = await _context.JobPositions.Include(j => j.Department).FirstAsync(j => j.Name == userData.JobPosition);//TODO: catch InvalidOperationException (when FirstAsync returns no elements)
        if(jobReference is null && (userData.JobPosition.IsNullOrEmpty() == false)) 
=======
        var jobReference = await _context
            .JobPositions
            .FirstAsync(j => j.Name == userData.JobPosition); //TODO: catch InvalidOperationException (when FirstAsync returns no elements)
        if (jobReference is null && (userData.JobPosition.IsNullOrEmpty() == false))
>>>>>>> d8b507ad3ea8983258d028f5675336339d783842
            return BadRequest("There are no job positions with that name");
        var _user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userData.Email);
        if (_user == null)
        {
            return Unauthorized("User not found");
        }
        _user.FirstName = userData.FirstName;
        _user.LastName = userData.LastName;
        _user.SelfDescription = userData.SelfDescription;
        _user.PictureUrl = userData.PictureUrl;
        _user.JobPosition = jobReference;
        _user.Username = GenerateUsername(_user.FirstName, _user.LastName);
        var user = new
        {
            _user.Email,
            _user.FirstName,
            _user.LastName,
            _user.Username,
            _user.SelfDescription,
            _user.JobPosition,
            _user.PictureUrl
        };
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return BadRequest("Update error");
        }
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] UserRegistrationModel userData)
    {
        //'@gmail.com' has 10 letters; email column has max length 50
        string pattern = @"^([A-Z]?|[a-z])[a-z]{0,19}\.([A-Z]?|[a-z])[a-z]{0,19}@gmail\.com";

        if (Regex.IsMatch(userData.Email, pattern) == false) //check email format
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
