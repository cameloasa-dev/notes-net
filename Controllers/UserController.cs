using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MyOwnApi.Models;

namespace MyOwnApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        // ---------------------------------------------------------
        // Inject UserManager — service from Identity
        // ---------------------------------------------------------
        public UserController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        // ---------------------------------------------------------
        // GET /api/user/me
        // ---------------------------------------------------------
        // USER DASHBOARD for future use.
        // return  user data  (from JWT).
        // ---------------------------------------------------------
        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var firstName = User.FindFirst("firstName")?.Value;
            var lastName = User.FindFirst("lastName")?.Value;

            return Ok(new {
                UserId = userId,
                Email = email,
                FirstName = firstName,
                LastName = lastName
            });
        }

        // ---------------------------------------------------------
        // GET /api/user
        // ---------------------------------------------------------
        // this is for debugging.
        // It is not for production, not for admin.
        // You use it only to see existing users.
        // ---------------------------------------------------------
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _userManager.Users
                .Select(u => new 
                {
                    u.Id,
                    u.UserName,
                    u.Email
                })
                .ToList();

            return Ok(users);
        }
    }
}
