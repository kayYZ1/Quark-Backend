using Microsoft.AspNetCore.Mvc;
using Quark_Backend.DAL;
using Quark_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Quark_Backend.Models;

namespace Quark_Backend.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ConversationController : ControllerBase
{
    private readonly QuarkDbContext _context;

    public ConversationController(QuarkDbContext context)
    {
        _context = context;
    }    

    public async Task ChangeConversationName(string name)
    {
        var conversation = await _context.Conversations.FirstAsync(c => c.Name == name);
        if(conversation is null)
            return;
        conversation.Name = name;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch(DbUpdateException)
        {
            
        }
    }
}