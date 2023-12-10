using Microsoft.AspNetCore.Mvc;
using Quark_Backend.DAL;
using Quark_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Quark_Backend.Models;

namespace Quark_Backend.Controllers
{
    //[Authorize(Policy = "PermissionLevel5")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AdminController : Controller
    {
        private readonly QuarkDbContext _dbContext;

        public AdminController(QuarkDbContext context)
        {
            _dbContext = context;
        }

        // User Management by admin


        [HttpDelete("/api/users/{Id}")] 
        public async Task<IActionResult> DeleteUser(int Id)
        {
            var userToDelete = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == Id);

            if (userToDelete == null)
            {
                return NotFound($"User with ID {Id} not found");
            }

            _dbContext.Users.Remove(userToDelete);
            _dbContext.SaveChanges();

            return Ok($"User '{userToDelete.FirstName} {userToDelete.LastName}' deleted successfully");
        }
        [HttpPost("/api/users/{Id}")]

        public IActionResult EditUser(int Id, [FromBody] UserEdit userEdit)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == Id);

            if (user == null)
            {
                return NotFound($"User with ID {Id} not found");
            }


            if (userEdit.JobId.HasValue)
            {
                var jobPosition = _dbContext.JobPositions.FirstOrDefault(j => j.Id == userEdit.JobId);
                if (jobPosition != null)
                {
                    user.JobPosition = jobPosition;
                }
                else
                {
                    return NotFound($"JobPosition with ID {userEdit.JobId} not found");
                }
            }

            if (userEdit.PermissionLevel.HasValue)
            {
                user.PermissionLevel = userEdit.PermissionLevel.Value;
            }
            if (userEdit.Email != null)
            {
                user.Email = userEdit.Email;
            }
            if (userEdit.Password != null)
            {
                user.Password = userEdit.Password;
            }
            try
            {
                _dbContext.SaveChanges();
                return Ok($"User with ID {Id} updated successfully");
            }
            catch (DbUpdateException)
            {
                return BadRequest($"Error updating user with ID {Id}");
            }
        }

        [HttpGet("latest/{userId}")]
        public IActionResult GetLatestMessages(int userId)
        {
            var userExists = _dbContext.Users.Any(u => u.Id == userId);
            if (!userExists)
            {
                return NotFound($"User with ID {userId} not found");
            }
            // Retrieve the latest 10 messages sent by the user
            try
            {
                var latestMessages = _dbContext.Messages
                    .Where(m => m.UserId == userId)
                    .OrderByDescending(m => m.SentDate)
                    .Take(10)
                    .ToList();

                return Ok(latestMessages);
            }
            catch (Exception ex)
            {
                // TODO: log exception
                Console.WriteLine($"Error retrieving messages: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }


        // Maybe better to implement on frontend ? 
        //[HttpPost]
        //public IActionResult ConfirmDeletion(int Id)
        //{
        //    var userToDelete = _dbContext.Users.Find(Id);

        //    if (userToDelete == null)
        //    {
        //        return NotFound($"User with ID {Id} not found");
        //    }

        //    var confirmationMessage = $"You're about to delete user '{userToDelete.FirstName} {userToDelete.LastName}'. Do you wish to proceed?";

        //    return Ok(new { ConfirmationMessage = confirmationMessage, UserId = userToDelete.Id });
        //}


    }
}
