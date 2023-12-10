using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Quark_Backend.Hubs;
using Quark_Backend.Models;

using Microsoft.EntityFrameworkCore;
using Quark_Backend.DAL;

namespace Quark_Backend.Controllers
{
    public class SignalRController : Controller
    {
        private readonly IHubContext<QuarkHub> _hubContext;
        private readonly QuarkDbContext _dbContext;
        public SignalRController(IHubContext<QuarkHub> hubContext, QuarkDbContext dbContext)
        {
            _dbContext = dbContext;
            _hubContext = hubContext;
        }

        [HttpGet]
        [Route("PushUser")]

        public async Task<IActionResult> PushUser(UserInfoModel user)
        {
            var _user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (_user == null)
            {
                return NotFound("User does not exist");
            }

            try
            {
                await _hubContext.Clients.All.SendAsync("ReceiveUser", user);

                return Ok("User information sent successfully");
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error sending user information: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }

        }
        [HttpGet]
        [Route("PushMessage")]

        public async Task<IActionResult> PushMessage(String message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
            return Ok("Success");
        }
    }
}
