using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Contexts;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Administrator")]
    public class AdminPanelController : Controller
    {
        // TODO Implement AdminPanel Controller from Trello task description
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminPanelController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }

        [HttpPost("CreateEmployee")]
        public async Task<IActionResult> CreateEmployee()
        {
            return Ok();
        }

        [HttpPost("CreateAdministrator")]
        public async Task<IActionResult> CreateAdministrator()
        {
            return Ok();
        }

        [HttpPut("EditEmployee")]
        public async Task<IActionResult> EditEmployee()
        {
            return Ok();
        }

        [HttpPut("EditAdministrator")]
        public async Task<IActionResult> EditAdministrator()
        {
            return Ok();
        }

        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(Guid? id)
        {
            return Ok();
        }

        [HttpGet("Users")]
        public IActionResult GetUsers()
        {
            var users = this._userManager.Users.ToList();
            return Ok(users);
        }

        [HttpGet("Roles")]
        public IActionResult GetRoles()
        {
            var roles = this._context.Roles.Select(r => r.Name).ToList();
            return Ok(roles);
        }

        [HttpGet("GetUserRoles/{userId}")]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            var user = await this._userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var userRoles = await this._userManager.GetRolesAsync(user);
            return Ok(userRoles);
        }
    }
}
