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
public class AnnouncementsController : ControllerBase
{
    private readonly QuarkDbContext _context;

    public AnnouncementsController(QuarkDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<object>>> GetAnnouncements()
    {
        List<Announcement> _announcements = await _context.Announcements.ToListAsync();
        var announcements = new List<object>();

        foreach (Announcement announcement in _announcements)
        {
            User userRef = await _context
                .Users
                .FirstOrDefaultAsync(u => u.Id == announcement.UserId);
            if (userRef != null)
            {
                var data = new
                {
                    Id = announcement.Id,
                    Title = announcement.Title,
                    Content = announcement.Content,
                    UserFirstName = userRef.FirstName,
                    UserLastName = userRef.LastName,
                    UserPictureUrl = userRef.PictureUrl,
                    Time = announcement.Time,
                };
                announcements.Add(data);
            }
        }
        return announcements;
    }

    [HttpPost]
    public async Task<IActionResult> AddAnnouncement([FromBody] AnnouncementModel _announcement)
    {
        var userRef = await _context.Users.FirstAsync(u => u.Email == _announcement.Email);
        var userFirstName = userRef.FirstName;
        var userLastName = userRef.LastName;
        var userPictureUrl = userRef.PictureUrl;
        var response = new
        {
            _announcement.Title,
            _announcement.Content,
            _announcement.Time,
            userFirstName,
            userLastName,
            userPictureUrl
        };

        Announcement announcement = new Announcement
        {
            Title = _announcement.Title,
            Content = _announcement.Content,
            Time = _announcement.Time,
            UserId = userRef.Id
        };

        _context.Add(announcement);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return BadRequest("Error while adding announcement");
        }

        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAnnouncement([FromBody] AnnouncementModel _announcement)
    {
        var userRef = await _context.Users.FirstAsync(u => u.Email == _announcement.Email);
        var announcement = await _context.Announcements.FirstAsync(a => a.Id == _announcement.Id);
        if (userRef.Id != announcement.UserId)
        {
            return BadRequest("You are not authorized to delete this announcement");
        }
        _context.Remove(announcement);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return BadRequest("Error while deleting announcement");
        }
        return Ok();
    }

}
